using UnityEngine;

public class SceneAvatarInitializer : MonoBehaviour
{
    public GameObject playerArmature; // アバターを配置する場所（PlayerArmature）

    private void Start()
    {
        // AvatarManagerから現在選択中のアバターPrefabを取得
        GameObject selectedAvatarPrefab = AvatarManager.Instance?.GetCurrentAvatar();

        if (selectedAvatarPrefab != null)
        {
            // PlayerArmature配下の既存の子オブジェクトを削除
            foreach (Transform child in playerArmature.transform)
            {
                Destroy(child.gameObject);
            }

            // 新しいアバターをPlayerArmatureの子として生成
            GameObject newAvatar = Instantiate(selectedAvatarPrefab, playerArmature.transform);

            // 生成したアバターの位置と回転をリセット
            newAvatar.transform.localPosition = Vector3.zero;
            newAvatar.transform.localRotation = Quaternion.identity;

            // 必要に応じてスケールもリセット（任意）
            newAvatar.transform.localScale = Vector3.one;

            Debug.Log("アバターが正しく配置されました。");
        }
        else
        {
            Debug.LogWarning("選択されたアバターが見つかりません！AvatarManagerが正しく設定されているか確認してください。");
        }
    }
}
