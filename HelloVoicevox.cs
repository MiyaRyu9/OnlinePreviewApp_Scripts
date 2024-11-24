using Cysharp.Threading.Tasks;

using UnityEngine;

using VOICEVOX;

[RequireComponent(typeof(AudioSource))]
public class HelloVoicevox : MonoBehaviour
{
    private void Start()
    {
        // VOICEVOXクライアント作成
        var voicevox = new Voicevox();

        // キャラクターの指定
        var speaker = Speaker.四国めたん_ノーマル;

        // 音声合成オプション
        var options = new SynthesisOptions() { SpeedScale = 1.1f };

        // AudioSourceコンポーネント取得
        var audioSource = GetComponent<AudioSource>();

        // 読み上げるテキスト
        const string text = "VOICEVOXは、商用・非商用問わず無料で使えるテキスト読み上げ・歌声合成ソフトウェアです。";

        UniTask.Void(async () =>
        {
            // テキストから音声を生成してAudioClipに変換
            var audioClip = await voicevox.SynthesisAsync(text, speaker, options, destroyCancellationToken);

            // AudioClipをセット
            audioSource.clip = audioClip;

            // 再生
            audioSource.Play();
        });
    }
}