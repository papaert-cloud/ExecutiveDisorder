using TMPro;
using UnityEngine;

public class InfoBoxUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_infoText;

    public void SetInfoUI(string infoText)
    {
        m_infoText.text = infoText;
    }
}
