using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class HurtFlash : MonoBehaviour
{
    [SerializeField] private Image flashImage;
    [SerializeField] private float flashDuration = 0.3f;
    [SerializeField] private float maxAlpha = 0.5f;
    [SerializeField] private bool playOnAwake = true;

    public Action OnAnimFinish;

    private void Awake()
    {
        PlayFlash();
    }

    public void PlayFlash()
    {
        if (flashImage == null) return;

        flashImage.DOKill(); // cancel any running tween
        flashImage.color = new Color(1f, 0f, 0f, 0f); // reset transparent

        // Fade in, then fade out
        flashImage.DOFade(maxAlpha, flashDuration * 0.5f)
            .OnComplete(() =>
            {
                flashImage.DOFade(0f, flashDuration * 0.5f)
                    .OnComplete(() => OnAnimFinish?.Invoke()); // invoke at the very end
            });
    }
}
