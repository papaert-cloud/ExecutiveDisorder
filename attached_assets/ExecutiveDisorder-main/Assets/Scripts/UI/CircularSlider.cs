using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CircularSlider : MonoBehaviour, IDragHandler
{
    [SerializeField] private Image fillImage;
    [SerializeField] private RectTransform handle;
    [Range(0f, 1f)] public float value; // 0 to 1

    [SerializeField] private bool CanDrag = false;

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag) return;
        Vector2 dir = eventData.position - (Vector2)fillImage.rectTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        value = angle / 360f;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        fillImage.fillAmount = value;
        float angle = value * 360f;
        handle.localEulerAngles = new Vector3(0, 0, angle);

    }
}
