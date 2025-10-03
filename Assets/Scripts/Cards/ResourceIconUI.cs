using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceIconUI : MonoBehaviour
{
    [SerializeField]
    private Image m_iconImage;
    [SerializeField]
    private TextMeshProUGUI m_resourceText;
    [SerializeField]
    private Image m_resourcePreviewIcon;

    public void SetResource(Sprite iconImage, string resourceText)
    {
        m_iconImage.sprite = iconImage;
        m_resourceText.text = resourceText;  
    }

    public void EnablePreviewIcon(Color previewColor)
    {
        m_resourcePreviewIcon.gameObject.SetActive(true);
        m_resourcePreviewIcon.color = previewColor;
    }
}
