using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static AudioClip ToAudioClip(byte[] wavFile)
    {
        using (MemoryStream stream = new MemoryStream(wavFile))
        {
            stream.Seek(0, SeekOrigin.Begin);
            return ToAudioClip(stream);
        }
    }

    public static AudioClip ToAudioClip(Stream wavStream)
    {
        using (BinaryReader reader = new BinaryReader(wavStream))
        {
            string riff = new string(reader.ReadChars(4));
            if (riff != "RIFF") throw new Exception("Invalid WAV file");

            reader.ReadInt32(); // File size
            string wave = new string(reader.ReadChars(4));
            if (wave != "WAVE") throw new Exception("Invalid WAV file");

            string fmt = new string(reader.ReadChars(4));
            if (fmt != "fmt ") throw new Exception("Invalid WAV file");

            reader.ReadInt32(); // Chunk size
            reader.ReadInt16(); // Audio format
            int channels = reader.ReadInt16();
            int sampleRate = reader.ReadInt32();
            reader.ReadInt32(); // Byte rate
            reader.ReadInt16(); // Block align
            int bitsPerSample = reader.ReadInt16();

            string data = new string(reader.ReadChars(4));
            if (data != "data") throw new Exception("Invalid WAV file");

            int dataSize = reader.ReadInt32();
            byte[] dataArray = reader.ReadBytes(dataSize);

            AudioClip audioClip = AudioClip.Create("AudioClip", dataSize / (bitsPerSample / 8), channels, sampleRate, false);
            float[] floatArray = new float[dataArray.Length / sizeof(short)];
            for (int i = 0; i < floatArray.Length; i++)
            {
                floatArray[i] = BitConverter.ToInt16(dataArray, i * sizeof(short)) / 32768.0f;
            }
            audioClip.SetData(floatArray, 0);

            return audioClip;
        }
    }
}
