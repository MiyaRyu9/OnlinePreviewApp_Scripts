using UnityEngine;
using Cysharp.Threading.Tasks;
using Claudia;
using VOICEVOX;
using TMPro;
using System.Threading;

[RequireComponent(typeof(AudioSource))]
public class LipSyncHandler : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource ���C���X�y�N�^�Ŏw��
    public Transform contentTransform; // ScrollView �� Content ���C���X�y�N�^�Ŏw��
    public GameObject messagePrefab;   // ���b�Z�[�W�\���p�̃v���n�u���C���X�y�N�^�Ŏw��

    private new CancellationToken destroyCancellationToken; // �L�����Z���g�[�N��

    public async void HandleInput(string message)
    {
        // ��b���O�Ƀ��[�U�[�̃��b�Z�[�W��ǉ�
        AddMessageToLog(message, true);

        // Claude ����̉������擾
        var response = await GetClaudeResponseAsync(message);
        if (!string.IsNullOrEmpty(response))
        {
            AddMessageToLog(response, false); // Claude �̉��������O�ɒǉ�

            // ���������ƍĐ�
            await PlayVoiceAsync(response);
        }
        else
        {
            Debug.LogError("Claude �̉�������ł��B");
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
        var speaker = Speaker.����Ց��Y_�т��т�;
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
            Debug.LogError("�K�v�ȃR���|�[�l���g���ݒ肳��Ă��܂���I");
            return;
        }

        // ���b�Z�[�W���O�ɒǉ�
        var messageObject = Instantiate(messagePrefab, contentTransform);
        var textComponent = messageObject.GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("Message Prefab �� TextMeshProUGUI �R���|�[�l���g������܂���I");
            return;
        }

        textComponent.text = message;
        textComponent.color = isUser ? Color.blue : Color.green;
    }
}
