using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;

namespace VOICEVOX
{
    [Serializable]
    public class SynthesisOptions
    {
        [Tooltip("話速")]
        public float SpeedScale = 1f;

        [Tooltip("音高")]
        public float PitchScale = 0f;

        [Tooltip("抑揚")]
        public float IntonationScale = 1f;

        [Tooltip("音量")]
        public float VolumeScale = 1f;
    }

    public class Voicevox
    {
        public string BaseAddress { get; set; } = "http://127.0.0.1:50021";

        public async UniTask<AudioClip> SynthesisAsync(string text, Speaker speaker, SynthesisOptions options, CancellationToken cancellationToken)
        {
            var data = await GetAudioQueryAsync(text, (int)speaker, cancellationToken);

            var node = JsonNode.Parse(data);

            node["speedScale"] = options.SpeedScale;
            node["pitchScale"] = options.PitchScale;
            node["intonationScale"] = options.IntonationScale;
            node["volumeScale"] = options.VolumeScale;

            return await GetAudioClipAsync(JsonSerializer.SerializeToUtf8Bytes(node), (int)speaker, cancellationToken);
        }

        internal async Task<AudioClip> SynthesisAsync(string content, Speaker speaker, SynthesisOptions options)
        {
            throw new NotImplementedException();
        }

        private async UniTask<AudioClip> GetAudioClipAsync(byte[] audioQuery, int speaker, CancellationToken cancellationToken)
        {
            var url = $"{BaseAddress}/synthesis?speaker={speaker}";

            using var uploadHandler = new UploadHandlerRaw(audioQuery) { contentType = "application/json" };

            using var downloadHandler = new DownloadHandlerAudioClip(url, AudioType.WAV);

            using var request = new UnityWebRequest(url, "POST", downloadHandler, uploadHandler);

            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            return downloadHandler.audioClip;
        }

        private async UniTask<byte[]> GetAudioQueryAsync(string text, int speaker, CancellationToken cancellationToken)
        {
            var encodedtext = HttpUtility.UrlEncode(text);

            var url = $"{BaseAddress}/audio_query?speaker={speaker}&text={encodedtext}";

            using var downloadHandler = new DownloadHandlerBuffer();

            using var uploadHandler = new UploadHandlerRaw(Array.Empty<byte>());

            using var request = new UnityWebRequest(url, "POST", downloadHandler, uploadHandler);

            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            return downloadHandler.data;
        }
    }

    public enum Speaker
    {
        // VOICEVOX
        No7_アナウンス = 30,
        No7_ノーマル = 29,
        No7_読み聞かせ = 31,
        WhiteCUL_かなしい = 25,
        WhiteCUL_たのしい = 24,
        WhiteCUL_ノーマル = 23,
        WhiteCUL_びえーん = 26,
        あいえるたん_ノーマル = 68,
        ずんだもん_あまあま = 1,
        ずんだもん_ささやき = 22,
        ずんだもん_セクシー = 5,
        ずんだもん_ツンツン = 7,
        ずんだもん_なみだめ = 76,
        ずんだもん_ノーマル = 3,
        ずんだもん_ヒソヒソ = 38,
        ずんだもん_ヘロヘロ = 75,
        ちび式じい_ノーマル = 42,
        ナースロボタイプＴ_ノーマル = 47,
        ナースロボタイプＴ_楽々 = 48,
        ナースロボタイプＴ_恐怖 = 49,
        ナースロボタイプＴ_内緒話 = 50,
        もち子さん_セクシーあん子 = 66,
        もち子さん_のんびり = 80,
        もち子さん_ノーマル = 20,
        もち子さん_喜び = 79,
        もち子さん_泣き = 77,
        もち子さん_怒り = 78,
        雨晴はう_ノーマル = 10,
        琴詠ニア_ノーマル = 74,
        九州そら_あまあま = 15,
        九州そら_ささやき = 19,
        九州そら_セクシー = 17,
        九州そら_ツンツン = 18,
        九州そら_ノーマル = 16,
        栗田まろん_ノーマル = 67,
        剣崎雌雄_ノーマル = 21,
        玄野武宏_ツンギレ = 40,
        玄野武宏_ノーマル = 11,
        玄野武宏_喜び = 39,
        玄野武宏_悲しみ = 41,
        後鬼_ぬいぐるみver = 28,
        後鬼_人間ver = 27,
        四国めたん_あまあま = 0,
        四国めたん_ささやき = 36,
        四国めたん_セクシー = 4,
        四国めたん_ツンツン = 6,
        四国めたん_ノーマル = 2,
        四国めたん_ヒソヒソ = 37,
        春歌ナナ_ノーマル = 54,
        春日部つむぎ_ノーマル = 8,
        小夜SAYO_ノーマル = 46,
        雀松朱司_ノーマル = 52,
        聖騎士紅桜_ノーマル = 51,
        青山龍星_かなしみ = 85,
        青山龍星_しっとり = 84,
        青山龍星_ノーマル = 13,
        青山龍星_喜び = 83,
        青山龍星_熱血 = 81,
        青山龍星_不機嫌 = 82,
        青山龍星_囁き = 86,
        中国うさぎ_おどろき = 62,
        中国うさぎ_こわがり = 63,
        中国うさぎ_ノーマル = 61,
        中国うさぎ_へろへろ = 64,
        猫使アル_うきうき = 57,
        猫使アル_おちつき = 56,
        猫使アル_ノーマル = 55,
        猫使ビィ_おちつき = 59,
        猫使ビィ_ノーマル = 58,
        猫使ビィ_人見知り = 60,
        波音リツ_クイーン = 65,
        波音リツ_ノーマル = 9,
        白上虎太郎_おこ = 34,
        白上虎太郎_びえーん = 35,
        白上虎太郎_びくびく = 33,
        白上虎太郎_ふつう = 12,
        白上虎太郎_わーい = 32,
        満別花丸_ささやき = 71,
        満別花丸_ノーマル = 69,
        満別花丸_ぶりっ子 = 72,
        満別花丸_ボーイ = 73,
        満別花丸_元気 = 70,
        冥鳴ひまり_ノーマル = 14,
        櫻歌ミコ_ノーマル = 43,
        櫻歌ミコ_ロリ = 45,
        櫻歌ミコ_第二形態 = 44,
        麒ヶ島宗麟_ノーマル = 53,
        // VOICEVOX Nemo
        女声1_ノーマル = 10005,
        女声2_ノーマル = 10007,
        女声3_ノーマル = 10004,
        女声4_ノーマル = 10003,
        女声5_ノーマル = 10008,
        女声6_ノーマル = 10006,
        男声1_ノーマル = 10001,
        男声2_ノーマル = 10000,
        男声3_ノーマル = 10002,
    }
}
