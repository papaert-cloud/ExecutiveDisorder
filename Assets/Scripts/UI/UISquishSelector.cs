using UnityEngine;
using DG.Tweening;

public class UISquishSelector : MonoBehaviour
{
    private Vector3 normalScale = Vector3.one;
    private Vector3 selectedScale = Vector3.one * 1.05f;
    private Tween currentTween;

    ///// <summary>
    ///// Plays the select animation and stays slightly scaled up.
    ///// </summary>
    //public void Select()
    //{
    //    if (currentTween != null && currentTween.IsActive()) currentTween.Kill();

    //    Sequence sequence = DOTween.Sequence();
    //    sequence.Append(transform.DOScale(new Vector3(0.9f, 1.1f, 1f), 0.1f))  // Squish
    //            .Append(transform.DOScale(new Vector3(1.1f, 0.9f, 1f), 0.1f))  // Squash
    //            .Append(transform.DOScale(selectedScale, 0.1f));              // Stay big

    //    currentTween = sequence;
    //}

    /// <summary>
    /// Plays a snappy select animation and holds the selected scale.
    /// </summary>
    public void Select()
    {
        if (currentTween != null && currentTween.IsActive()) currentTween.Kill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(0.95f, 1.05f, 1f), 0.08f))   // Slight inward squish
                .Append(transform.DOScale(selectedScale, 0.08f).SetEase(Ease.OutQuad)); // Pop outward firmly

        currentTween = sequence;
    }

    /// <summary>
    /// Reverts the scale back to normal.
    /// </summary>
    public void Deselect()
    {
        if (currentTween != null && currentTween.IsActive()) currentTween.Kill();

        currentTween = transform.DOScale(normalScale, 0.15f).SetEase(Ease.OutBack);
    }

    public void Switch(bool active)
    {
        if (active) Select();
        else Deselect();
    }
}
