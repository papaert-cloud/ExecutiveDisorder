using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_resourceText;
    [SerializeField] TextMeshProUGUI m_previewResourceText;
    [SerializeField] Image m_previewResourceImage;
    [SerializeField] GameObject m_resourcePreviewHolder;
    [SerializeField] private bool m_isPreviewActive = false;
    [SerializeField] private float m_resourceValueAnimDuration = 1.0f;

    [SerializeField] private Color m_positiveColor;
    [SerializeField] private Color m_negativeColor;
    [SerializeField] private Color m_neutralColor;

    [SerializeField] private Transform m_magnitudeHolder;
    [SerializeField] private Image m_magnitudeImage;

    private Tween currentTween;
    private float currentAmount;

    private Coroutine m_hideCoroutine;
    private bool isHiding = false;
    private bool bufferShowRequest = false;
    private float bufferedAmount;

    private bool m_showValue;

    public Action OnHideCompleted;

    private void Start()
    {
        m_resourcePreviewHolder.GetComponent<UIPopDownAnimator>().OnHide += (() => { OnHideCompleted?.Invoke(); });
        EndingManager.Instance.OnEnding += OnEnd;
    }

    private void OnEnd(EndingSO ending)
    {
        currentTween?.Kill();
    }

    public void ShowResourcePreview(float amount, bool showValue)
    {
        m_showValue = showValue;

        if (isHiding)
        {
            // Buffer this call until hide finishes
            bufferShowRequest = true;
            bufferedAmount = amount;
            return;
        }

        // Cancel pending hide if it's still in delay phase
        if (m_hideCoroutine != null)
        {
            StopCoroutine(m_hideCoroutine);
            m_hideCoroutine = null;
        }

        if (m_isPreviewActive)
        {
            SetPreviewText(amount);
        }
        else
        {
            m_isPreviewActive = true;
            SetPreviewText(amount);
            m_resourcePreviewHolder.GetComponent<UIPopDownAnimator>().Show();
        }
    }

    public void SetPreviewText(float amount)
    {

        Color previewColor = (amount >= 0) ? m_positiveColor : m_negativeColor;
        m_previewResourceImage.color = previewColor;

        if (m_showValue)
        {
            m_previewResourceImage.gameObject.SetActive(true);
            m_magnitudeHolder.gameObject.SetActive(false);
            string prefix = (amount >= 0) ? "+" : "";
            m_previewResourceText.text = $"{prefix}{amount}";
        } else
        {
            m_magnitudeHolder.gameObject.SetActive(true);
            m_previewResourceImage.gameObject.SetActive(false);
            m_magnitudeImage.color = previewColor;
            int magnitudeImageSize = GetMagnitudeImageSize(Mathf.Abs(amount));
            m_magnitudeImage.GetComponent<RectTransform>().sizeDelta = new Vector2 (magnitudeImageSize, magnitudeImageSize);
        }
    }

    private int GetMagnitudeImageSize(float amount)
    {
        if (amount < 7) return 15;
        if (amount < 12) return 20;
        if (amount < 16) return 25;
        if (amount >= 16) return 30;
        return 20;
    }

    public void HideResourcePreview()
    {
        // Cancel any running coroutine first
        if (m_hideCoroutine != null)
        {
            StopCoroutine(m_hideCoroutine);
        }

        // Cancel any pending buffered Show request
        if (bufferShowRequest)
        {
            bufferShowRequest = false;
            bufferedAmount = 0;
        }

        m_hideCoroutine = StartCoroutine(DelayedHide());
    }

    private System.Collections.IEnumerator DelayedHide()
    {
        yield return new WaitForSeconds(0.2f);

        isHiding = true;

        m_resourcePreviewHolder.GetComponent<UIPopDownAnimator>().Hide();

        // Wait for the animation duration before clearing flags
        yield return new WaitForSeconds(m_resourcePreviewHolder.GetComponent<UIPopDownAnimator>().OutDuration); // match your DOTween hide animation time

        m_isPreviewActive = false;
        isHiding = false;
        m_hideCoroutine = null;

        if (bufferShowRequest)
        {
            bufferShowRequest = false;
            ShowResourcePreview(bufferedAmount, m_showValue);
        }
    }

    public void UpdateResourceText(string displayName, float amount)
    {
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        float currentValue = currentAmount;

        currentAmount = amount;

        currentTween = DOTween.To(() => currentValue, x => {
            currentValue = x;
            m_resourceText.text = $"{displayName}: {Mathf.RoundToInt(currentValue)}";
        }, amount, m_resourceValueAnimDuration)
        .SetEase(Ease.Linear)
        .OnComplete(() => currentTween = null);

    }


    public bool IsPreviewActive()
    {
        return m_isPreviewActive;
    }
}
