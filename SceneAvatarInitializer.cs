using UnityEngine;

public class SceneAvatarInitializer : MonoBehaviour
{
    public GameObject playerArmature; // �A�o�^�[��z�u����ꏊ�iPlayerArmature�j

    private void Start()
    {
        // AvatarManager���猻�ݑI�𒆂̃A�o�^�[Prefab���擾
        GameObject selectedAvatarPrefab = AvatarManager.Instance?.GetCurrentAvatar();

        if (selectedAvatarPrefab != null)
        {
            // PlayerArmature�z���̊����̎q�I�u�W�F�N�g���폜
            foreach (Transform child in playerArmature.transform)
            {
                Destroy(child.gameObject);
            }

            // �V�����A�o�^�[��PlayerArmature�̎q�Ƃ��Đ���
            GameObject newAvatar = Instantiate(selectedAvatarPrefab, playerArmature.transform);

            // ���������A�o�^�[�̈ʒu�Ɖ�]�����Z�b�g
            newAvatar.transform.localPosition = Vector3.zero;
            newAvatar.transform.localRotation = Quaternion.identity;

            // �K�v�ɉ����ăX�P�[�������Z�b�g�i�C�Ӂj
            newAvatar.transform.localScale = Vector3.one;

            Debug.Log("�A�o�^�[���������z�u����܂����B");
        }
        else
        {
            Debug.LogWarning("�I�����ꂽ�A�o�^�[��������܂���IAvatarManager���������ݒ肳��Ă��邩�m�F���Ă��������B");
        }
    }
}
