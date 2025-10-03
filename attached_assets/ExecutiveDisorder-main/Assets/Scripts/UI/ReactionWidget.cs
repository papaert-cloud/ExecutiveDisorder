using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ReactionWidget : MonoBehaviour
{
    [Header("Refs (self)")]
    [SerializeField] private RectTransform crowdRoot;   // root that moves up/down/shakes (usually this transform)
    [SerializeField] private Image crowdImage;
    [SerializeField] private RectTransform bubbleRoot;  // child of crowdRoot
    [SerializeField] private CanvasGroup bubbleGroup;
    [SerializeField] private TextMeshProUGUI bubbleText;

    [Header("Motion Settings")]
    [SerializeField] private float riseDistance = 120f;
    [SerializeField] private float riseDuration = 0.25f;
    [SerializeField] private float shakeDuration = 0.35f;
    [SerializeField] private Vector2 shakeStrength = new Vector2(20f, 0f);
    [SerializeField] private int shakeVibrato = 20;
    [SerializeField] private float bubbleFadeDuration = 0.2f;
    [SerializeField] private float bubbleHoldDuration = 1.1f;
    [SerializeField] private float fallDuration = 0.25f;
    [SerializeField] private Ease riseEase = Ease.OutBack;
    [SerializeField] private Ease fallEase = Ease.InBack;

    private Sequence _seq;

    private void OnDisable()
    {
        _seq?.Kill();
        _seq = null;
    }

    /// <summary>
    /// Kicks off the reaction animation and destroys the widget at the end.
    /// </summary>
    public void Play(Sprite sprite, RectTransform anchor, Vector2 bubbleLocalOffset, string message, Action onFinish = null)
    {
        if (anchor == null || crowdRoot == null) { Destroy(gameObject); return; }

        if (crowdImage != null && sprite != null) crowdImage.sprite = sprite;

        // Copy anchoring from anchor so we’re positioned relative to it
        crowdRoot.pivot = anchor.pivot;
        crowdRoot.anchorMin = anchor.anchorMin;
        crowdRoot.anchorMax = anchor.anchorMax;

        // Start hidden below
        var startPos = anchor.anchoredPosition - new Vector2(0f, riseDistance);
        var upPos = startPos + new Vector2(0f, riseDistance);
        crowdRoot.anchoredPosition = startPos;

        // Bubble (local to crowdRoot)
        if (bubbleRoot != null) bubbleRoot.anchoredPosition = bubbleLocalOffset;
        if (bubbleText != null) bubbleText.text = message;
        if (bubbleGroup != null) bubbleGroup.alpha = 0f;

        gameObject.SetActive(true);

        _seq?.Kill();
        _seq = DOTween.Sequence();

        _seq.Append(crowdRoot.DOAnchorPos(upPos, riseDuration).SetEase(riseEase));
        _seq.Append(crowdRoot.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato));

        if (bubbleGroup != null) _seq.Append(bubbleGroup.DOFade(1f, bubbleFadeDuration));
        else _seq.AppendInterval(bubbleFadeDuration);

        _seq.AppendInterval(bubbleHoldDuration);

        if (bubbleGroup != null) _seq.Append(bubbleGroup.DOFade(0f, bubbleFadeDuration));
        else _seq.AppendInterval(bubbleFadeDuration);

        _seq.Append(crowdRoot.DOAnchorPos(startPos, fallDuration).SetEase(fallEase));

        _seq.OnComplete(() =>
        {
            onFinish?.Invoke();
            Destroy(gameObject);
        });
    }
}
