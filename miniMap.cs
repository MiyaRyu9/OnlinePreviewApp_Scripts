using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public RectTransform playerMarker; // �~�j�}�b�v��̃}�[�J�[
    public Transform player;          // �v���C���[�i�L�����N�^�[�j��Transform
    public RectTransform minimap;     // �~�j�}�b�v��RectTransform

    public Vector2 mapSize = new Vector2(10, 10); // ���[���h��̒n�}�T�C�Y�iX=��, Y=�����j

    void Update()
    {
        // ���[���h���W���擾
        Vector3 playerPosition = player.position;

        // ���[���h���W��0�`1�͈̔͂ɐ��K��
        float normalizedX = (playerPosition.x + mapSize.x / 2) / mapSize.x;
        float normalizedZ = (playerPosition.z + mapSize.y / 2) / mapSize.y;

        // �~�j�}�b�v�̃��[�J�����W�ɕϊ�
        float markerX = normalizedX * minimap.rect.width - minimap.rect.width / 2;
        float markerY = normalizedZ * minimap.rect.height - minimap.rect.height / 2;

        // �}�[�J�[�̈ʒu���X�V
        playerMarker.anchoredPosition = new Vector2(markerX, markerY);
    }


void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("");
        GUILayout.EndVertical(); // EndVertical��Y���ƃG���[������
    }

}
