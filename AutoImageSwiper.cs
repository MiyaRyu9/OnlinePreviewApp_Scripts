using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoImageSwiper : MonoBehaviour
{
    public ScrollRect scrollRect; // Scroll View
    public GameObject pageIndicatorParent; // ドットをまとめたオブジェクト
    public Color activeColor = Color.white; // アクティブなドットの色
    public Color inactiveColor = Color.gray; // 非アクティブなドットの色
    public float swipeInterval = 3f;
    public float swipeSpeed = 0.5f;

    private int currentIndex = 0;
    private int totalImages;
    private Image[] dots;

    void Start()
    {
        // Scroll ViewとContentの設定確認
        if (scrollRect == null || scrollRect.content == null)
        {
            Debug.LogError("ScrollRectまたはContentが設定されていません！");
            return;
        }

        // ページ数（Contentの子要素の数）を取得
        totalImages = scrollRect.content.childCount;

        // ページインジケーターのドットを配列に格納
        dots = pageIndicatorParent.GetComponentsInChildren<Image>();
        if (dots.Length != totalImages)
        {
            Debug.LogError("ドットの数とページ数が一致していません！");
            return;
        }

        UpdatePageIndicator(); // 初期状態を更新
        StartCoroutine(AutoSwipe());
    }

    IEnumerator AutoSwipe()
    {
        while (true)
        {
            yield return new WaitForSeconds(swipeInterval);
            currentIndex = (currentIndex + 1) % totalImages;
            float targetPosition = (float)currentIndex / (totalImages - 1);
            yield return StartCoroutine(SmoothScrollTo(targetPosition));
            UpdatePageIndicator(); // ページインジケーターを更新
        }
    }

    IEnumerator SmoothScrollTo(float targetPosition)
    {
        float start = scrollRect.horizontalNormalizedPosition;
        float elapsedTime = 0f;

        while (elapsedTime < swipeSpeed)
        {
            elapsedTime += Time.deltaTime;
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(start, targetPosition, elapsedTime / swipeSpeed);
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = targetPosition;
    }

    void UpdatePageIndicator()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].color = (i == currentIndex) ? activeColor : inactiveColor;
        }
    }
}
