using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager Instance; // �V���O���g���C���X�^���X

    private GameObject currentAvatar; // �I�����ꂽ�A�o�^�[

    private void Awake()
    {
        // �V���O���g���p�^�[���̐ݒ�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[�����܂����Ńf�[�^��ێ�
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �A�o�^�[��ݒ�
    public void SetCurrentAvatar(GameObject avatar)
    {
        currentAvatar = avatar;
    }

    // ���݂̃A�o�^�[���擾
    public GameObject GetCurrentAvatar()
    {
        return currentAvatar;
    }
}
