using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CrisisController : MonoBehaviour
{
    public TextMeshProUGUI crisisTitle;
    public TextMeshProUGUI crisisDescription;
    public Image crisisImage;
    public Button continueButton;
    public float autoProgressDelay = 8f;

    void Start()
    {
        if (crisisTitle != null)
            crisisTitle.text = "NUCLEAR CRISIS ALERT";
            
        if (crisisDescription != null)
        {
            crisisDescription.text = "Mr. President, we have an emergency situation!\n\n" +
                "Our intelligence reports indicate unusual activity at a hostile nation's nuclear facility. " +
                "The military advisors are calling for immediate action. " +
                "The media is already speculating. The stock market is plummeting. " +
                "The people are looking to you for leadership.\n\n" +
                "Every decision you make from this moment will have consequences. " +
                "Can you navigate this crisis and maintain control?";
        }
        
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinue);
            
        StartCoroutine(AutoProgress());
    }

    IEnumerator AutoProgress()
    {
        yield return new WaitForSeconds(autoProgressDelay);
        OnContinue();
    }

    void OnContinue()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
