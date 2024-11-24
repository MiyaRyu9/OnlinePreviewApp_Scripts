using Cysharp.Threading.Tasks;
using UnityEngine;
using Claudia;
using VOICEVOX;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ClaudeToVoiceVoxWithMicButton : MonoBehaviour
{
    private DictationRecognizer dictationRecognizer; // Windows�̉����F��
    public AudioSource audioSource; // �����Đ��p��AudioSource
    public Button micButton; // �}�C�N�{�^��
    private bool isRecording = false; // �^�������ǂ����̃t���O

    private void Start()
    {
        // DictationRecognizer�̏�����
        dictationRecognizer = new DictationRecognizer();

        // �����F�����ʂ��������郊�X�i�[��ݒ�
        dictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.Log("�F�����ꂽ�e�L�X�g: " + text);
            SendToClaude(text).Forget(); // �F�������e�L�X�g��Claude�ɑ��M
        };

        // �����F���G���[���������郊�X�i�[
        dictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogError("�����F���G���[: " + error);
        };

        // �{�^���ɃN���b�N���X�i�[��ǉ�
        micButton.onClick.AddListener(ToggleRecording);
    }

    private void ToggleRecording()
    {
        if (isRecording)
        {
            // �^�����Ȃ�A�^�����~
            dictationRecognizer.Stop();
            isRecording = false;
            Debug.Log("�^����~");
        }
        else
        {
            // �^�����łȂ��Ȃ�A�^�����J�n
            dictationRecognizer.Start();
            isRecording = true;
            Debug.Log("�^���J�n");
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
        });

        // Claude�̉������e���擾
        string content = response.Content.ToString();
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
            var audioClip = await voicevox.SynthesisAsync(content, speaker, options, destroyCancellationToken);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Claude�̉�������ł��B");
        }
    }

    private void OnDestroy()
    {
        // �����F�����~���A���\�[�X�����
        dictationRecognizer?.Stop();
        dictationRecognizer?.Dispose();
    }
}
