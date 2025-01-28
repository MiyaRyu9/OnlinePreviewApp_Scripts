using UnityEngine;
using UnityEngine.UI;

public class AvatarButton : MonoBehaviour
{
    public GameObject[] avatarCards; // アバターカードのリスト
    private int selectedIndex = -1;

    public void SelectAvatar(int index)
    {
        // 前回の選択をリセット
        if (selectedIndex >= 0)
            avatarCards[selectedIndex].GetComponent<Outline>().enabled = false;

        // 新しい選択を強調
        selectedIndex = index;
        avatarCards[selectedIndex].GetComponent<Outline>().enabled = true;
    }
}
