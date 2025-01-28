using UnityEngine;
using UnityEngine.UI;

public class AvatarButton : MonoBehaviour
{
    public GameObject[] avatarCards; // �A�o�^�[�J�[�h�̃��X�g
    private int selectedIndex = -1;

    public void SelectAvatar(int index)
    {
        // �O��̑I�������Z�b�g
        if (selectedIndex >= 0)
            avatarCards[selectedIndex].GetComponent<Outline>().enabled = false;

        // �V�����I��������
        selectedIndex = index;
        avatarCards[selectedIndex].GetComponent<Outline>().enabled = true;
    }
}
