using UnityEngine;
using Cysharp.Threading.Tasks;
using Claudia;
using VOICEVOX;
using TMPro;
using System.Threading;

[RequireComponent(typeof(AudioSource))]
public class LipSyncHandler : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource をインスペクタで指定
    public Transform contentTransform; // ScrollView の Content をインスペクタで指定
    public GameObject messagePrefab;   // メッセージ表示用のプレハブをインスペクタで指定

    private new CancellationToken destroyCancellationToken; // キャンセルトークン

    public async void HandleInput(string message)
    {
        // 会話ログにユーザーのメッセージを追加
        AddMessageToLog(message, true);

        // Claude からの応答を取得
        var response = await GetClaudeResponseAsync(message);
        if (!string.IsNullOrEmpty(response))
        {
            AddMessageToLog(response, false); // Claude の応答をログに追加

            // 音声合成と再生
            await PlayVoiceAsync(response);
        }
        else
        {
            Debug.LogError("Claude の応答が空です。");
        }
    }

    private async UniTask<string> GetClaudeResponseAsync(string message)
    {
        var anthropic = new Anthropic();
        var messages = new Message[]
        {
            new() { Role = Roles.User, Content = message }
        };

        var response = await anthropic.Messages.CreateAsync(new()
        {
            Model = Models.Claude3Haiku,
            MaxTokens = 1024,
            Messages = messages,
        },
        cancellationToken: destroyCancellationToken);

        return response.Content.ToString();
    }

    private async UniTask PlayVoiceAsync(string content)
    {
        var voicevox = new Voicevox();
        var speaker = Speaker.白上虎太郎_びくびく;
        var options = new SynthesisOptions() { SpeedScale = 1.0f };

        var audioClip = await voicevox.SynthesisAsync(content, speaker, options, destroyCancellationToken);
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    private void AddMessageToLog(string message, bool isUser)
    {
        if (messagePrefab == null || contentTransform == null)
        {
            Debug.LogError("必要なコンポーネントが設定されていません！");
            return;
        }

        // メッセージログに追加
        var messageObject = Instantiate(messagePrefab, contentTransform);
        var textComponent = messageObject.GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("Message Prefab に TextMeshProUGUI コンポーネントがありません！");
            return;
        }

        textComponent.text = message;
        textComponent.color = isUser ? Color.blue : Color.green;
    }
}
