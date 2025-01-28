using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoImageSwiper : MonoBehaviour
{
    public ScrollRect scrollRect; // Scroll View
    public GameObject pageIndicatorParent; // �h�b�g���܂Ƃ߂��I�u�W�F�N�g
    public Color activeColor = Color.white; // �A�N�e�B�u�ȃh�b�g�̐F
    public Color inactiveColor = Color.gray; // ��A�N�e�B�u�ȃh�b�g�̐F
    public float swipeInterval = 3f;
    public float swipeSpeed = 0.5f;

    private int currentIndex = 0;
    private int totalImages;
    private Image[] dots;

    void Start()
    {
        // Scroll View��Content�̐ݒ�m�F
        if (scrollRect == null || scrollRect.content == null)
        {
            Debug.LogError("ScrollRect�܂���Content���ݒ肳��Ă��܂���I");
            return;
        }

        // �y�[�W���iContent�̎q�v�f�̐��j���擾
        totalImages = scrollRect.content.childCount;

        // �y�[�W�C���W�P�[�^�[�̃h�b�g��z��Ɋi�[
        dots = pageIndicatorParent.GetComponentsInChildren<Image>();
        if (dots.Length != totalImages)
        {
            Debug.LogError("�h�b�g�̐��ƃy�[�W������v���Ă��܂���I");
            return;
        }

        UpdatePageIndicator(); // ������Ԃ��X�V
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
            UpdatePageIndicator(); // �y�[�W�C���W�P�[�^�[���X�V
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
