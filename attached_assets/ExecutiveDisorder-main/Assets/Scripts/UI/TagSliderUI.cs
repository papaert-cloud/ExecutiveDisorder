using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TagSliderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Tag;
    [SerializeField] private Slider m_Slider;
    [SerializeField] private TextMeshProUGUI m_valueText;

    [SerializeField] private Color m_LowColor;
    [SerializeField] private Color m_HighColor;

    private const float MinValue = 30f;
    private const float MaxValue = 70f;
    private Image m_FillImage;

    private void Awake()
    {
        if (m_Slider.fillRect != null)
            m_FillImage = m_Slider.fillRect.GetComponent<Image>();
    }

    public void SetTagSliderUI(string tag, int value)
    {
        m_Tag.text = tag;
        m_valueText.text = value.ToString();

        value = Mathf.Clamp(value, (int)MinValue, (int)MaxValue);

        float normalized = Mathf.InverseLerp(MinValue, MaxValue, value);
        Color targetColor = Color.Lerp(m_LowColor, m_HighColor, normalized);

        // Animate value
        m_Slider.DOValue(value, 0.5f).SetEase(Ease.OutQuad);

        // Animate color
        DOTween.To(() => m_FillImage.color, x => m_FillImage.color = x, targetColor, 0.5f).SetEase(Ease.OutQuad);
    }
}
