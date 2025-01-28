using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPanel; // 吹き出しパネル (Panel内)
    public TextMeshProUGUI tooltipText; // 吹き出し用テキスト
    public string houseInfo; // 表示する家の情報

    public RectTransform canvasRect; // CanvasのRectTransform

    void Start()
    {
        // Tooltipパネルを初期状態で非表示に
        tooltipPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 吹き出しを有効化し、情報を更新
        tooltipPanel.SetActive(true);
        tooltipText.text = houseInfo;

        // 吹き出しの位置を調整
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            eventData.pressEventCamera,
            out localPoint
        );
        tooltipPanel.transform.localPosition = localPoint + new Vector2(50, 50); // 少しずらして表示
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 吹き出しを非表示
        tooltipPanel.SetActive(false);
    }
}
