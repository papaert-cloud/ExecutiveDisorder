using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmojiReactUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action PointerEnter;
    public Action PointerExit;

    [SerializeField] Button m_EmojiButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_EmojiButton.IsInteractable())
        {
            AnimateScaleUp();
            PointerEnter?.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_EmojiButton.IsInteractable())
        {
            AnimateScaleDown();
            PointerExit?.Invoke();
        }
    }

    public void AnimateScaleUp()
    {
        transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack);
    }

    public void AnimateScaleDown()
    {
        transform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine);
    }

}
