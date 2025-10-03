using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MediaResourceUI : MonoBehaviour
{
    [SerializeField] private Image resourceIcon;
    [SerializeField] private TextMeshProUGUI resoruceText;

    public void SetResourceUI(ResourceType resourceType, string value)
    {
        resourceIcon.sprite = GameAssetsManager.Instance.GetResourceIcon(resourceType);
        string displayName = GameAssetsManager.Instance.GetResourceTypeName(resourceType);
        resoruceText.text = $"{displayName}: {value}";
    }
}
