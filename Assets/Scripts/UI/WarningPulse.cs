using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class WarningPulse : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image warningImage;   // The red warning triangle image
    [SerializeField] private Image veilImage;      // The black veil overlay image
    [SerializeField] private TextMeshProUGUI warningText;

    [Header("Pulse Settings")]
    [SerializeField] private float warningMinAlpha = 0.3f;
    [SerializeField] private float warningMaxAlpha = 1f;
    [SerializeField] private float warningPulseDuration = 0.8f; // seconds

    [Space(10)]
    [SerializeField] private float veilMinAlpha = 0.1f;
    [SerializeField] private float veilMaxAlpha = 0.5f;
    [SerializeField] private float veilPulseDuration = 2f; // slower pace

    public Action OnAnimFinish;

    private void Start()
    {
        StartAnim();
    }

    private void StartAnim(float totalDuration = 5f) // X seconds
    {
        AudioManager.Instance.PlaySFXFor(AudioManager.SoundType.AlienSiren, totalDuration, 0f, 0.5f, 1, 0.95f);
        AudioManager.Instance.PlaySFXFor(AudioManager.SoundType.Siren, totalDuration, 0f, 0.5f, 0.1f, 0.95f);

        if (warningImage != null)
        {
            warningImage.DOKill();

            // calculate how many loops fit in totalDuration
            int loops = Mathf.RoundToInt(totalDuration / warningPulseDuration);

            warningImage.DOFade(warningMaxAlpha, warningPulseDuration)
                .SetLoops(loops, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .From(warningMinAlpha)
                .OnComplete(() => OnAnimFinish?.Invoke());
        }

        if (veilImage != null)
        {
            veilImage.DOKill();

            int loops = Mathf.RoundToInt(totalDuration / veilPulseDuration);

            veilImage.DOFade(veilMaxAlpha, veilPulseDuration)
                .SetLoops(loops, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .From(veilMinAlpha);
        }
    }

}
