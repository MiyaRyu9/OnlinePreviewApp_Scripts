using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class LipSyncController : MonoBehaviour
{
    public AudioSource targetAudioSource; // 監視するAudioSource
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private int mouthOpenBlendShapeIndex;

    void Start()
    {
        // アバターのSkinnedMeshRendererを取得
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        // BlendShapeのインデックスを取得 (MouthOpenという名前を仮定)
        mouthOpenBlendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("MouthOpen");

        if (mouthOpenBlendShapeIndex == -1)
        {
            Debug.LogError("BlendShape 'MouthOpen' が見つかりません！");
        }

        if (targetAudioSource == null)
        {
            Debug.LogError("AudioSourceが設定されていません！");
        }
    }

    void Update()
    {
        // エラーチェック：BlendShapeインデックスが有効か
        if (mouthOpenBlendShapeIndex == -1)
        {
            return; // エラーが出た場合は処理をスキップ
        }

        // エラーチェック：AudioSourceが設定されているか
        if (targetAudioSource == null)
        {
            return; // AudioSourceが設定されていない場合は処理をスキップ
        }

        if (targetAudioSource.isPlaying)
        {
            // 音声波形データを取得
            float[] audioData = new float[256];
            targetAudioSource.GetOutputData(audioData, 0);

            // 配列が空でないか確認
            if (audioData.Length == 0)
            {
                Debug.LogWarning("Audio data is empty.");
                return;
            }

            // 平均音量を計算してBlendShapeを更新
            float averageVolume = Mathf.Abs(audioData.Average()) * 100f; // 音量スケール調整
            skinnedMeshRenderer.SetBlendShapeWeight(mouthOpenBlendShapeIndex, Mathf.Clamp(averageVolume, 0, 100));
        }
        else
        {
            // 音声が再生されていない場合はBlendShapeをリセット
            skinnedMeshRenderer.SetBlendShapeWeight(mouthOpenBlendShapeIndex, 0);
        }
    }
}
