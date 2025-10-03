using DG.Tweening;
using System;
using UnityEngine;

public class UIPopDownAnimator : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_textRect;
    [SerializeField]
    private float m_moveDistance = 50f;
    [SerializeField]
    private float m_inDuration = 0.3f;
    public float OutDuration = 0.1f;
    [SerializeField]
    private Ease m_ease = Ease.OutBack;

    private Vector2 m_originalPos;

    public Action OnHide;

    private void Awake()
    {
        if (m_textRect == null)
            m_textRect = GetComponent<RectTransform>();

        m_originalPos = m_textRect.anchoredPosition;
        m_textRect.gameObject.SetActive(false);
    }

    public void Show()
    {
        m_textRect.gameObject.SetActive(true);
        m_textRect.anchoredPosition = m_originalPos + Vector2.up * m_moveDistance;
        m_textRect.DOAnchorPos(m_originalPos, m_inDuration).SetEase(m_ease);
    }

    public void Hide()
    {
        m_textRect.DOAnchorPos(m_originalPos + Vector2.up * m_moveDistance, OutDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() => { m_textRect.gameObject.SetActive(false); OnHide?.Invoke(); });
    }
}
