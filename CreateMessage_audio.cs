using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Claudia;
using VOICEVOX;
using System.Threading;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class ClaudeToVoiceVox : MonoBehaviour
{
    private CancellationTokenSource cancellationTokenSource; // キャンセルトークンの宣言
    public AudioSource audioSource; // AudioSourceをインスペクタで指定
    public InputField inputField; // InputFieldをインスペクタで指定
    public Button submitButton; // Submitボタンをインスペクタで指定

    // 会話ログ関連
    public Transform contentTransform; // ScrollViewのContentをインスペクタで指定
    public GameObject messagePrefab;   // メッセージ表示用のプレハブをインスペクタで指定
    public GameObject avatar; // メニューで選択されたアバターのインスタンス
    public Transform avatarSpawnPoint; // アバターの生成位置をインスペクタで指定

    private void Start()
    {
        // キャンセルトークンの初期化
        cancellationTokenSource = new CancellationTokenSource();

        // Nullチェック
        ValidateInspectorFields();

        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnDestroy()
    {
        // キャンセルトークンの解放
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
    }

    private void ValidateInspectorFields()
    {
        if (audioSource == null) Debug.LogError("AudioSourceが設定されていません！");
        if (inputField == null) Debug.LogError("InputFieldが設定されていません！");
        if (submitButton == null) Debug.LogError("SubmitButtonが設定されていません！");
        if (contentTransform == null) Debug.LogError("Content Transformが設定されていません！");
        if (messagePrefab == null) Debug.LogError("Message Prefabが設定されていません！");
        if (avatarSpawnPoint == null) Debug.LogError("Avatar Spawn Pointが設定されていません！");
    }

    private void OnSubmit()
    {
        string userInput = inputField.text;
        if (!string.IsNullOrEmpty(userInput))
        {
            AddMessageToLog(userInput, true); // ユーザーのメッセージをログに追加
            SendToClaude(userInput).Forget();
            inputField.text = "";
        }
        else
        {
            Debug.LogWarning("入力が空です。");
        }
    }

    private async UniTaskVoid SendToClaude(string message)
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
        cancellationToken: cancellationTokenSource.Token);

        string content = response.Content.ToString();
        Debug.Log(content);

        AddMessageToLog(content, false); // Claudeの応答をログに追加

        var voicevox = new Voicevox();
        var speaker = Speaker.九州そら_セクシー;
        var options = new SynthesisOptions() { SpeedScale = 1.5f };

        if (!string.IsNullOrEmpty(content))
        {
            var audioClip = await voicevox.SynthesisAsync(content, speaker, options, cancellationTokenSource.Token);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Claudeの応答が空です。");
        }
    }

    private void AddMessageToLog(string message, bool isUser)
    {
        if (messagePrefab == null || contentTransform == null)
        {
            Debug.LogError("必要なコンポーネントが設定されていません！");
            return;
        }

        // プレハブを生成してContentに配置
        var messageObject = Instantiate(messagePrefab, contentTransform);

        // メッセージテキストの設定
        var textComponent = messageObject.GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("Message PrefabにTextMeshProUGUIコンポーネントがありません！");
            return;
        }

        textComponent.text = message;
        textComponent.color = isUser ? Color.blue : Color.green;

        // ScrollViewを最新メッセージにスクロール
        Canvas.ForceUpdateCanvases();
        var scrollRect = contentTransform.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 0f;
        }
        else
        {
            Debug.LogWarning("ScrollRectが見つかりませんでした！");
        }
    }

    private void SetupAvatarLipSync()
    {
        if (avatar != null)
        {
            var lipSyncController = avatar.GetComponent<LipSyncController>();
            if (lipSyncController != null)
            {
                // AudioSourceをアバターに渡す
                lipSyncController.targetAudioSource = audioSource;
            }
            else
            {
                Debug.LogWarning("アバターにLipSyncControllerがアタッチされていません！");
            }
        }
        else
        {
            Debug.LogError("アバターが設定されていません！");
        }
    }

    public void OnAvatarSelected(GameObject selectedAvatarPrefab)
    {
        // 選択したアバターをインスタンス化
        GameObject instantiatedAvatar = Instantiate(selectedAvatarPrefab, avatarSpawnPoint.position, Quaternion.identity);

        // ClaudeToVoiceVoxにアバターを設定
        avatar = instantiatedAvatar;

        // リップシンク設定を適用
        SetupAvatarLipSync();
    }
}
