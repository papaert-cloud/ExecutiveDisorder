using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class NewsTicker : MonoBehaviour
{
    public RectTransform textRect;
    public RectTransform containerRect;

    public float scrollDuration = 10f;
    public bool SrollOnStart = false;

    private Tween scrollTween; // Store the tween reference

    void Start()
    {
        if (SrollOnStart)
            StartScrolling();
    }

    public void StartScrolling()
    {
        StartCoroutine(StartScroll());
    }

    IEnumerator StartScroll()
    {
        //Wait for next fram in case text need to adjust its width
        yield return null;
        float textWidth = textRect.rect.width;
        float containerWidth = containerRect.rect.width;

        Vector2 startPos = new Vector2(containerWidth, textRect.anchoredPosition.y);
        Vector2 endPos = new Vector2(-textWidth, textRect.anchoredPosition.y);

        textRect.anchoredPosition = startPos;

        // Kill any existing tween to prevent overlap
        scrollTween?.Kill();

        scrollTween = textRect.DOAnchorPos(endPos, scrollDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    public void StopScrolling()
    {
        scrollTween?.Kill();
        scrollTween = null;
    }
}
