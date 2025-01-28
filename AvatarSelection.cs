using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarSelection : MonoBehaviour
{
    public GameObject avatarPrefab;

    public void SelectAvatar(GameObject avatarPrefab)
    {
        AvatarManager.Instance.SetCurrentAvatar(avatarPrefab); // 選択したアバターを保存
        
    }
}
