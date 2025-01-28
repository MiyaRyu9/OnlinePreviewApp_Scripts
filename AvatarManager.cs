using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager Instance; // シングルトンインスタンス

    private GameObject currentAvatar; // 選択されたアバター

    private void Awake()
    {
        // シングルトンパターンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでデータを保持
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // アバターを設定
    public void SetCurrentAvatar(GameObject avatar)
    {
        currentAvatar = avatar;
    }

    // 現在のアバターを取得
    public GameObject GetCurrentAvatar()
    {
        return currentAvatar;
    }
}
