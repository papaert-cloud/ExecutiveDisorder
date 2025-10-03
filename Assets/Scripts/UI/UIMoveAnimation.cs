using DG.Tweening;
using System;
using UnityEngine;

public class UIMoveAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private Vector2 pointA;
    [SerializeField] private Vector2 pointB;
    [SerializeField] private Vector2 pointC;
    [SerializeField] private float durationAB = 1f;
    [SerializeField] private float durationCA = 1f;

    public Action OnOutAnimComplete;

    private Tween currentTween;

    public void PlayAtoB()
    {
        KillCurrentTween();
        target.anchoredPosition = pointA;

        currentTween = target.DOAnchorPos(pointB, durationAB)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                OnOutAnimComplete?.Invoke();
            });
    }

    public void PlayCtoA()
    {
        KillCurrentTween();
        // Start at C (in case it's not)
        target.anchoredPosition = pointC;

        currentTween = target.DOAnchorPos(pointA, durationCA)
            .SetEase(Ease.OutSine);
    }

    private void KillCurrentTween()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
            currentTween = null;
        }
    }
}
