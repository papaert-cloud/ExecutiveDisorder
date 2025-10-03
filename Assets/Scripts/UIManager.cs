using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("Resource Bars")]
    public Slider popularityBar;
    public Slider stabilityBar;
    public Slider mediaBar;
    public Slider economicBar;
    
    public TextMeshProUGUI popularityText;
    public TextMeshProUGUI stabilityText;
    public TextMeshProUGUI mediaText;
    public TextMeshProUGUI economicText;
    
    [Header("Decision Card UI")]
    public GameObject cardPanel;
    public TextMeshProUGUI cardTitleText;
    public TextMeshProUGUI cardDescriptionText;
    public Image cardImage;
    public Button option1Button;
    public Button option2Button;
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    
    [Header("News & Social Media")]
    public GameObject newsPanel;
    public TextMeshProUGUI newsHeadlineText;
    public GameObject socialMediaPanel;
    public Transform socialMediaContainer;
    public GameObject socialMediaPostPrefab;
    
    [Header("Visual Effects")]
    public Image flashOverlay;
    public ParticleSystem[] effectParticles;
    
    [Header("Ending UI")]
    public GameObject endingPanel;
    public TextMeshProUGUI endingTitleText;
    public TextMeshProUGUI endingDescriptionText;
    public TextMeshProUGUI finalStatsText;

    private DecisionCard currentCard;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (option1Button != null)
            option1Button.onClick.AddListener(() => OnDecisionMade(true));
        if (option2Button != null)
            option2Button.onClick.AddListener(() => OnDecisionMade(false));
            
        UpdateResourceBars();
        ShowNextCard();
    }

    void Update()
    {
        if (GameManager.Instance != null)
        {
            UpdateResourceBars();
        }
    }

    void UpdateResourceBars()
    {
        if (GameManager.Instance == null) return;
        
        GameResources resources = GameManager.Instance.resources;
        
        if (popularityBar != null)
        {
            popularityBar.value = resources.popularity / 100f;
            if (popularityText != null)
                popularityText.text = $"Popularity: {resources.popularity:F0}%";
        }
        
        if (stabilityBar != null)
        {
            stabilityBar.value = resources.stability / 100f;
            if (stabilityText != null)
                stabilityText.text = $"Stability: {resources.stability:F0}%";
        }
        
        if (mediaBar != null)
        {
            mediaBar.value = resources.media / 100f;
            if (mediaText != null)
                mediaText.text = $"Media: {resources.media:F0}%";
        }
        
        if (economicBar != null)
        {
            economicBar.value = resources.economic / 100f;
            if (economicText != null)
                economicText.text = $"Economic: {resources.economic:F0}%";
        }
    }

    public void ShowNextCard()
    {
        if (GameManager.Instance == null) return;
        
        currentCard = GameManager.Instance.DrawCard();
        
        if (currentCard == null) return;
        
        if (cardPanel != null)
            cardPanel.SetActive(true);
            
        if (cardTitleText != null)
            cardTitleText.text = currentCard.cardTitle;
            
        if (cardDescriptionText != null)
            cardDescriptionText.text = currentCard.description;
            
        if (cardImage != null && currentCard.cardImage != null)
            cardImage.sprite = currentCard.cardImage;
            
        if (option1Text != null)
            option1Text.text = currentCard.option1Text;
            
        if (option2Text != null)
            option2Text.text = currentCard.option2Text;
    }

    void OnDecisionMade(bool isOption1)
    {
        if (currentCard == null || GameManager.Instance == null) return;
        
        GameManager.Instance.MakeDecision(currentCard, isOption1);
        
        StartCoroutine(ShowConsequencesAndNextCard());
    }

    IEnumerator ShowConsequencesAndNextCard()
    {
        // Show flash effect
        if (flashOverlay != null)
        {
            StartCoroutine(FlashEffect());
        }
        
        // Show news headlines
        UpdateNewsHeadlines();
        
        yield return new WaitForSeconds(1.5f);
        
        ShowNextCard();
    }

    IEnumerator FlashEffect()
    {
        Color startColor = flashOverlay.color;
        startColor.a = 0.5f;
        flashOverlay.color = startColor;
        
        float duration = 0.5f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Color color = flashOverlay.color;
            color.a = Mathf.Lerp(0.5f, 0f, elapsed / duration);
            flashOverlay.color = color;
            yield return null;
        }
        
        Color endColor = flashOverlay.color;
        endColor.a = 0f;
        flashOverlay.color = endColor;
    }

    void UpdateNewsHeadlines()
    {
        if (GameManager.Instance == null) return;
        
        List<string> headlines = GameManager.Instance.GetRecentHeadlines();
        if (headlines.Count > 0 && newsHeadlineText != null)
        {
            newsHeadlineText.text = "BREAKING: " + headlines[headlines.Count - 1];
            StartCoroutine(HideNewsAfterDelay());
        }
    }

    IEnumerator HideNewsAfterDelay()
    {
        if (newsPanel != null)
            newsPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        if (newsPanel != null)
            newsPanel.SetActive(false);
    }

    public void ShowVisualEffect(string effectName)
    {
        // Trigger particle effects based on effect name
        switch (effectName.ToLower())
        {
            case "explosion":
                if (effectParticles.Length > 0 && effectParticles[0] != null)
                    effectParticles[0].Play();
                break;
            case "celebration":
                if (effectParticles.Length > 1 && effectParticles[1] != null)
                    effectParticles[1].Play();
                break;
            case "crisis":
                if (effectParticles.Length > 2 && effectParticles[2] != null)
                    effectParticles[2].Play();
                break;
        }
    }

    public void ShowEnding(string endingType)
    {
        if (endingPanel != null)
        {
            endingPanel.SetActive(true);
            
            if (endingTitleText != null)
                endingTitleText.text = GetEndingTitle(endingType);
                
            if (endingDescriptionText != null)
                endingDescriptionText.text = GetEndingDescription(endingType);
                
            if (finalStatsText != null && GameManager.Instance != null)
            {
                GameResources res = GameManager.Instance.resources;
                finalStatsText.text = $"Final Stats:\n" +
                    $"Popularity: {res.popularity:F0}%\n" +
                    $"Stability: {res.stability:F0}%\n" +
                    $"Media Support: {res.media:F0}%\n" +
                    $"Economic Health: {res.economic:F0}%\n" +
                    $"Decisions Made: {GameManager.Instance.decisionsCount}";
            }
        }
        
        if (cardPanel != null)
            cardPanel.SetActive(false);
    }

    string GetEndingTitle(string endingType)
    {
        switch (endingType)
        {
            case "GoldenAge": return "THE GOLDEN AGE";
            case "Successful": return "SUCCESSFUL TERM";
            case "Balanced": return "BALANCED LEADERSHIP";
            case "Struggling": return "STRUGGLING THROUGH";
            case "Crisis": return "CONSTANT CRISIS";
            case "Disaster": return "COMPLETE DISASTER";
            case "Overthrown": return "OVERTHROWN";
            case "Collapse": return "NATION COLLAPSED";
            case "Censored": return "MEDIA BLACKOUT";
            case "Bankrupt": return "ECONOMIC RUIN";
            default: return "GAME OVER";
        }
    }

    string GetEndingDescription(string endingType)
    {
        switch (endingType)
        {
            case "GoldenAge": 
                return "Your leadership brought unprecedented prosperity and happiness. History will remember you as one of the greatest leaders.";
            case "Successful": 
                return "Through wise decisions, you navigated the challenges successfully. The nation thrives under your guidance.";
            case "Balanced": 
                return "You maintained a delicate balance between competing interests. Not perfect, but respectable.";
            case "Struggling": 
                return "Your tenure was marked by constant struggles. The nation survives, but barely.";
            case "Crisis": 
                return "Crisis after crisis defined your leadership. The people wonder how things went so wrong.";
            case "Disaster": 
                return "Your decisions led to catastrophe after catastrophe. Your name becomes synonymous with failure.";
            case "Overthrown": 
                return "The people rose up against you. Your unpopularity became unbearable, and you were removed from power.";
            case "Collapse": 
                return "The nation couldn't withstand the instability. Society has broken down completely.";
            case "Censored": 
                return "You lost all media support. Unable to communicate with the people, your government fell silent.";
            case "Bankrupt": 
                return "The economy collapsed under your watch. The nation faces financial ruin.";
            default: 
                return "Your leadership has come to an end.";
        }
    }

    public void OnPlayAgainButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNewGame();
        }
    }

    public void OnMainMenuButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}
