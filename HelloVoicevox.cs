using Cysharp.Threading.Tasks;

using UnityEngine;

using VOICEVOX;

[RequireComponent(typeof(AudioSource))]
public class HelloVoicevox : MonoBehaviour
{
    private void Start()
    {
        // VOICEVOX�N���C�A���g�쐬
        var voicevox = new Voicevox();

        // �L�����N�^�[�̎w��
        var speaker = Speaker.�l���߂���_�m�[�}��;

        // ���������I�v�V����
        var options = new SynthesisOptions() { SpeedScale = 1.1f };

        // AudioSource�R���|�[�l���g�擾
        var audioSource = GetComponent<AudioSource>();

        // �ǂݏグ��e�L�X�g
        const string text = "VOICEVOX�́A���p�E�񏤗p��킸�����Ŏg����e�L�X�g�ǂݏグ�E�̐������\�t�g�E�F�A�ł��B";

        UniTask.Void(async () =>
        {
            // �e�L�X�g���特���𐶐�����AudioClip�ɕϊ�
            var audioClip = await voicevox.SynthesisAsync(text, speaker, options, destroyCancellationToken);

            // AudioClip���Z�b�g
            audioSource.clip = audioClip;

            // �Đ�
            audioSource.Play();
        });
    }
}