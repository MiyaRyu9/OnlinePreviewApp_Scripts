
using Cysharp.Threading.Tasks;
using UnityEngine;
using Claudia;
using VOICEVOX;
using System.Threading;
using UnityEngine.UI; // UI�p�̖��O���

[RequireComponent(typeof(AudioSource))]
public class ClaudeToVoiceVox : MonoBehaviour
{
    private CancellationToken destroyCancellationToken; // �L�����Z���g�[�N���̐錾
    public AudioSource audioSource; // AudioSource���C���X�y�N�^�Ŏw��
    public InputField inputField; // InputField���C���X�y�N�^�Ŏw��
    public Button submitButton; // Submit�{�^�����C���X�y�N�^�Ŏw��

    private void Start()
    {
        // �{�^���ɃN���b�N���X�i�[��ǉ�
        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        // InputField����e�L�X�g���擾���AClaude�ɑ��M
        string userInput = inputField.text;
        if (!string.IsNullOrEmpty(userInput))
        {
            SendToClaude(userInput).Forget(); // �񓯊��������Ăяo��
            inputField.text = ""; // ���M���InputField���N���A
        }
        else
        {
            Debug.LogWarning("���͂���ł��B");
        }
    }

    private async UniTaskVoid SendToClaude(string message)
    {
        // Claude API�N���C�A���g�̍쐬
        var anthropic = new Anthropic();

        // ���b�Z�[�W�̔z��쐬
        var messages = new Message[]
        {
            new() { Role = Roles.User, Content = message }
        };

        // Claude API�Ń��N�G�X�g�𑗐M���A���X�|���X����M����
        var response = await anthropic.Messages.CreateAsync(new()
        {
            Model = Models.Claude3Haiku,
            MaxTokens = 1024,
            Messages = messages,
        },
        cancellationToken: destroyCancellationToken);

        // Claude�̉������e���擾
        string content = response.Content.ToString();

        // �������e�̕\��
        Debug.Log(content);

        // VOICEVOX�N���C�A���g�̍쐬
        var voicevox = new Voicevox();

        // �L�����N�^�[�̎w��
        var speaker = Speaker.�l���߂���_�m�[�}��;

        // ���������I�v�V����
        var options = new SynthesisOptions() { SpeedScale = 1.0f };

        // ��������łȂ��ꍇ�͉����������s��
        if (!string.IsNullOrEmpty(content))
        {
            // �e�L�X�g���特���𐶐�����AudioClip�ɕϊ�
            var audioClip = await voicevox.SynthesisAsync(content, speaker, options, destroyCancellationToken);

            // AudioClip���Z�b�g���čĐ�
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Claude�̉�������ł��B");
        }
    }
}
