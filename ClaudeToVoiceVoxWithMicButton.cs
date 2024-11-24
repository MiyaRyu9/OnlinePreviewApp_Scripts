using Cysharp.Threading.Tasks;
using UnityEngine;
using Claudia;
using VOICEVOX;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ClaudeToVoiceVoxWithMicButton : MonoBehaviour
{
    private DictationRecognizer dictationRecognizer; // Windowsの音声認識
    public AudioSource audioSource; // 音声再生用のAudioSource
    public Button micButton; // マイクボタン
    private bool isRecording = false; // 録音中かどうかのフラグ

    private void Start()
    {
        // DictationRecognizerの初期化
        dictationRecognizer = new DictationRecognizer();

        // 音声認識結果を処理するリスナーを設定
        dictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.Log("認識されたテキスト: " + text);
            SendToClaude(text).Forget(); // 認識したテキストをClaudeに送信
        };

        // 音声認識エラーを処理するリスナー
        dictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogError("音声認識エラー: " + error);
        };

        // ボタンにクリックリスナーを追加
        micButton.onClick.AddListener(ToggleRecording);
    }

    private void ToggleRecording()
    {
        if (isRecording)
        {
            // 録音中なら、録音を停止
            dictationRecognizer.Stop();
            isRecording = false;
            Debug.Log("録音停止");
        }
        else
        {
            // 録音中でないなら、録音を開始
            dictationRecognizer.Start();
            isRecording = true;
            Debug.Log("録音開始");
        }
    }




    private async UniTaskVoid SendToClaude(string message)
    {
        // Claude APIクライアントの作成
        var anthropic = new Anthropic();

        // メッセージの配列作成
        var messages = new Message[]
        {
            new() { Role = Roles.User, Content = message }
        };

        // Claude APIでリクエストを送信し、レスポンスを受信する
        var response = await anthropic.Messages.CreateAsync(new()
        {
            Model = Models.Claude3Haiku,
            MaxTokens = 1024,
            Messages = messages,
        });

        // Claudeの応答内容を取得
        string content = response.Content.ToString();
        Debug.Log(content);

        // VOICEVOXクライアントの作成
        var voicevox = new Voicevox();

        // キャラクターの指定
        var speaker = Speaker.四国めたん_ノーマル;

        // 音声合成オプション
        var options = new SynthesisOptions() { SpeedScale = 1.0f };

        // 応答が空でない場合は音声合成を行う
        if (!string.IsNullOrEmpty(content))
        {
            var audioClip = await voicevox.SynthesisAsync(content, speaker, options, destroyCancellationToken);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Claudeの応答が空です。");
        }
    }

    private void OnDestroy()
    {
        // 音声認識を停止し、リソースを解放
        dictationRecognizer?.Stop();
        dictationRecognizer?.Dispose();
    }
}
