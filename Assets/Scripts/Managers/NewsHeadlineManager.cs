using UnityEngine;
using TMPro;

public class NewsHeadlineManager : MonoBehaviour
{
    public static NewsHeadlineManager Instance;

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI headlineText;  // Drag your TMP text here

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Sets the headline text instantly.
    /// </summary>
    public void SetHeadline(string newHeadline)
    {
        if (headlineText == null)
        {
            Debug.LogWarning("[NewsHeadlineManager] No headlineText assigned!");
            return;
        }

        headlineText.text = newHeadline;
    }

    /// <summary>
    /// Clears the headline.
    /// </summary>
    public void ClearHeadline()
    {
        if (headlineText != null)
            headlineText.text = string.Empty;
    }

    /// <summary>
    /// Get the current headline.
    /// </summary>
    public string GetHeadline()
    {
        return headlineText != null ? headlineText.text : "";
    }
}
