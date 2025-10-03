using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TVPowerEffect : MonoBehaviour
{
    public RectTransform tvLine; // The white line
    public bool isOn = false;

    public Action OnAnimationFinish;

    public void ToggleTV()
    {
        StartCoroutine(StartAnim());
    }

    IEnumerator StartAnim()
    {
        yield return null;
        if (isOn)
        {
            // Turn off - shrink to a line then fade out
            tvLine.DOScaleY(0.05f, 0.3f).SetEase(Ease.InOutQuad);
            tvLine.GetComponent<Image>().DOFade(0, 0.2f).SetDelay(0.3f);
        }
        else
        {
            // Turn on - fade in then expand
            tvLine.GetComponent<Image>().DOFade(1, 0.1f);
            tvLine.DOScaleY(1f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.1f).OnComplete(() => OnAnimationFinish?.Invoke());
        }

        isOn = !isOn;
    }
}
