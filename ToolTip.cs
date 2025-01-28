using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPanel; // �����o���p�l�� (Panel��)
    public TextMeshProUGUI tooltipText; // �����o���p�e�L�X�g
    public string houseInfo; // �\������Ƃ̏��

    public RectTransform canvasRect; // Canvas��RectTransform

    void Start()
    {
        // Tooltip�p�l����������ԂŔ�\����
        tooltipPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // �����o����L�������A�����X�V
        tooltipPanel.SetActive(true);
        tooltipText.text = houseInfo;

        // �����o���̈ʒu�𒲐�
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            eventData.pressEventCamera,
            out localPoint
        );
        tooltipPanel.transform.localPosition = localPoint + new Vector2(50, 50); // �������炵�ĕ\��
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // �����o�����\��
        tooltipPanel.SetActive(false);
    }
}
