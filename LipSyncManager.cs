using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class LipSyncController : MonoBehaviour
{
    public AudioSource targetAudioSource; // �Ď�����AudioSource
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private int mouthOpenBlendShapeIndex;

    void Start()
    {
        // �A�o�^�[��SkinnedMeshRenderer���擾
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        // BlendShape�̃C���f�b�N�X���擾 (MouthOpen�Ƃ������O������)
        mouthOpenBlendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("MouthOpen");

        if (mouthOpenBlendShapeIndex == -1)
        {
            Debug.LogError("BlendShape 'MouthOpen' ��������܂���I");
        }

        if (targetAudioSource == null)
        {
            Debug.LogError("AudioSource���ݒ肳��Ă��܂���I");
        }
    }

    void Update()
    {
        // �G���[�`�F�b�N�FBlendShape�C���f�b�N�X���L����
        if (mouthOpenBlendShapeIndex == -1)
        {
            return; // �G���[���o���ꍇ�͏������X�L�b�v
        }

        // �G���[�`�F�b�N�FAudioSource���ݒ肳��Ă��邩
        if (targetAudioSource == null)
        {
            return; // AudioSource���ݒ肳��Ă��Ȃ��ꍇ�͏������X�L�b�v
        }

        if (targetAudioSource.isPlaying)
        {
            // �����g�`�f�[�^���擾
            float[] audioData = new float[256];
            targetAudioSource.GetOutputData(audioData, 0);

            // �z�񂪋�łȂ����m�F
            if (audioData.Length == 0)
            {
                Debug.LogWarning("Audio data is empty.");
                return;
            }

            // ���ω��ʂ��v�Z����BlendShape���X�V
            float averageVolume = Mathf.Abs(audioData.Average()) * 100f; // ���ʃX�P�[������
            skinnedMeshRenderer.SetBlendShapeWeight(mouthOpenBlendShapeIndex, Mathf.Clamp(averageVolume, 0, 100));
        }
        else
        {
            // �������Đ�����Ă��Ȃ��ꍇ��BlendShape�����Z�b�g
            skinnedMeshRenderer.SetBlendShapeWeight(mouthOpenBlendShapeIndex, 0);
        }
    }
}
