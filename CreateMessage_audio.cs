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
    private CancellationTokenSource cancellationTokenSource; // �L�����Z���g�[�N���̐錾
    public AudioSource audioSource; // AudioSource���C���X�y�N�^�Ŏw��
    public InputField inputField; // InputField���C���X�y�N�^�Ŏw��
    public Button submitButton; // Submit�{�^�����C���X�y�N�^�Ŏw��

    // ��b���O�֘A
    public Transform contentTransform; // ScrollView��Content���C���X�y�N�^�Ŏw��
    public GameObject messagePrefab;   // ���b�Z�[�W�\���p�̃v���n�u���C���X�y�N�^�Ŏw��
    public GameObject avatar; // ���j���[�őI�����ꂽ�A�o�^�[�̃C���X�^���X
    public Transform avatarSpawnPoint; // �A�o�^�[�̐����ʒu���C���X�y�N�^�Ŏw��

    private void Start()
    {
        // �L�����Z���g�[�N���̏�����
        cancellationTokenSource = new CancellationTokenSource();

        // Null�`�F�b�N
        ValidateInspectorFields();

        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnDestroy()
    {
        // �L�����Z���g�[�N���̉��
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
    }

    private void ValidateInspectorFields()
    {
        if (audioSource == null) Debug.LogError("AudioSource���ݒ肳��Ă��܂���I");
        if (inputField == null) Debug.LogError("InputField���ݒ肳��Ă��܂���I");
        if (submitButton == null) Debug.LogError("SubmitButton���ݒ肳��Ă��܂���I");
        if (contentTransform == null) Debug.LogError("Content Transform���ݒ肳��Ă��܂���I");
        if (messagePrefab == null) Debug.LogError("Message Prefab���ݒ肳��Ă��܂���I");
        if (avatarSpawnPoint == null) Debug.LogError("Avatar Spawn Point���ݒ肳��Ă��܂���I");
    }

    private void OnSubmit()
    {
        string userInput = inputField.text;
        if (!string.IsNullOrEmpty(userInput))
        {
            AddMessageToLog(userInput, true); // ���[�U�[�̃��b�Z�[�W�����O�ɒǉ�
            SendToClaude(userInput).Forget();
            inputField.text = "";
        }
        else
        {
            Debug.LogWarning("���͂���ł��B");
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

        AddMessageToLog(content, false); // Claude�̉��������O�ɒǉ�

        var voicevox = new Voicevox();
        var speaker = Speaker.��B����_�Z�N�V�[;
        var options = new SynthesisOptions() { SpeedScale = 1.5f };

        if (!string.IsNullOrEmpty(content))
        {
            var audioClip = await voicevox.SynthesisAsync(content, speaker, options, cancellationTokenSource.Token);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Claude�̉�������ł��B");
        }
    }

    private void AddMessageToLog(string message, bool isUser)
    {
        if (messagePrefab == null || contentTransform == null)
        {
            Debug.LogError("�K�v�ȃR���|�[�l���g���ݒ肳��Ă��܂���I");
            return;
        }

        // �v���n�u�𐶐�����Content�ɔz�u
        var messageObject = Instantiate(messagePrefab, contentTransform);

        // ���b�Z�[�W�e�L�X�g�̐ݒ�
        var textComponent = messageObject.GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("Message Prefab��TextMeshProUGUI�R���|�[�l���g������܂���I");
            return;
        }

        textComponent.text = message;
        textComponent.color = isUser ? Color.blue : Color.green;

        // ScrollView���ŐV���b�Z�[�W�ɃX�N���[��
        Canvas.ForceUpdateCanvases();
        var scrollRect = contentTransform.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 0f;
        }
        else
        {
            Debug.LogWarning("ScrollRect��������܂���ł����I");
        }
    }

    private void SetupAvatarLipSync()
    {
        if (avatar != null)
        {
            var lipSyncController = avatar.GetComponent<LipSyncController>();
            if (lipSyncController != null)
            {
                // AudioSource���A�o�^�[�ɓn��
                lipSyncController.targetAudioSource = audioSource;
            }
            else
            {
                Debug.LogWarning("�A�o�^�[��LipSyncController���A�^�b�`����Ă��܂���I");
            }
        }
        else
        {
            Debug.LogError("�A�o�^�[���ݒ肳��Ă��܂���I");
        }
    }

    public void OnAvatarSelected(GameObject selectedAvatarPrefab)
    {
        // �I�������A�o�^�[���C���X�^���X��
        GameObject instantiatedAvatar = Instantiate(selectedAvatarPrefab, avatarSpawnPoint.position, Quaternion.identity);

        // ClaudeToVoiceVox�ɃA�o�^�[��ݒ�
        avatar = instantiatedAvatar;

        // ���b�v�V���N�ݒ��K�p
        SetupAvatarLipSync();
    }
}
