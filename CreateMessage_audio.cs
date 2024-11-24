
using Cysharp.Threading.Tasks;
using UnityEngine;
using Claudia;
using VOICEVOX;
using System.Threading;
using UnityEngine.UI; // UI用の名前空間

[RequireComponent(typeof(AudioSource))]
public class ClaudeToVoiceVox : MonoBehaviour
{
    private CancellationToken destroyCancellationToken; // キャンセルトークンの宣言
    public AudioSource audioSource; // AudioSourceをインスペクタで指定
    public InputField inputField; // InputFieldをインスペクタで指定
    public Button submitButton; // Submitボタンをインスペクタで指定

    private void Start()
    {
        // ボタンにクリックリスナーを追加
        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        // InputFieldからテキストを取得し、Claudeに送信
        string userInput = inputField.text;
        if (!string.IsNullOrEmpty(userInput))
        {
            SendToClaude(userInput).Forget(); // 非同期処理を呼び出し
            inputField.text = ""; // 送信後にInputFieldをクリア
        }
        else
        {
            Debug.LogWarning("入力が空です。");
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
        },
        cancellationToken: destroyCancellationToken);

        // Claudeの応答内容を取得
        string content = response.Content.ToString();

        // 応答内容の表示
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
            // テキストから音声を生成してAudioClipに変換
            var audioClip = await voicevox.SynthesisAsync(content, speaker, options, destroyCancellationToken);

            // AudioClipをセットして再生
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Claudeの応答が空です。");
        }
    }
}
