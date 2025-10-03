using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameResources resources;
    public PoliticalCharacter currentCharacter;
    public List<DecisionCard> allCards = new List<DecisionCard>();
    public List<DecisionCard> cardDeck = new List<DecisionCard>();
    public int decisionsCount = 0;
    public int maxDecisions = 50;
    
    private List<string> newsHeadlines = new List<string>();
    private List<string> socialMediaPosts = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (resources == null)
        {
            resources = new GameResources();
        }
        LoadAllCards();
        ShuffleDeck();
    }

    void LoadAllCards()
    {
        allCards.Clear();
        DecisionCard[] loadedCards = Resources.LoadAll<DecisionCard>("DecisionCards");
        allCards.AddRange(loadedCards);
    }

    void ShuffleDeck()
    {
        cardDeck = new List<DecisionCard>(allCards);
        for (int i = 0; i < cardDeck.Count; i++)
        {
            DecisionCard temp = cardDeck[i];
            int randomIndex = Random.Range(i, cardDeck.Count);
            cardDeck[i] = cardDeck[randomIndex];
            cardDeck[randomIndex] = temp;
        }
    }

    public DecisionCard DrawCard()
    {
        if (cardDeck.Count == 0)
        {
            ShuffleDeck();
        }
        
        DecisionCard card = cardDeck[0];
        cardDeck.RemoveAt(0);
        return card;
    }

    public void MakeDecision(DecisionCard card, bool isOption1)
    {
        decisionsCount++;
        
        DecisionConsequence[] consequences = isOption1 ? card.option1Consequences : card.option2Consequences;
        
        foreach (var consequence in consequences)
        {
            resources.ModifyResource(consequence.resourceName, consequence.amount);
            
            if (!string.IsNullOrEmpty(consequence.newsHeadline))
            {
                AddNewsHeadline(consequence.newsHeadline);
            }
            
            if (!string.IsNullOrEmpty(consequence.socialMediaReaction))
            {
                AddSocialMediaPost(consequence.socialMediaReaction);
            }
            
            if (!string.IsNullOrEmpty(consequence.visualEffect))
            {
                TriggerVisualEffect(consequence.visualEffect);
            }
            
            if (!string.IsNullOrEmpty(consequence.audioClip))
            {
                PlayAudioEffect(consequence.audioClip);
            }
        }
        
        // Check for cascades
        if (card.cascadeCards != null && card.cascadeCards.Length > 0)
        {
            if (Random.value <= card.cascadeProbability)
            {
                DecisionCard cascadeCard = card.cascadeCards[Random.Range(0, card.cascadeCards.Length)];
                cardDeck.Insert(0, cascadeCard);
            }
        }
        
        // Check game over conditions
        if (resources.IsGameOver() || decisionsCount >= maxDecisions)
        {
            EndGame();
        }
    }

    public void AddNewsHeadline(string headline)
    {
        newsHeadlines.Add(headline);
        if (newsHeadlines.Count > 10) newsHeadlines.RemoveAt(0);
    }

    public void AddSocialMediaPost(string post)
    {
        socialMediaPosts.Add(post);
        if (socialMediaPosts.Count > 15) socialMediaPosts.RemoveAt(0);
    }

    public List<string> GetRecentHeadlines()
    {
        return new List<string>(newsHeadlines);
    }

    public List<string> GetRecentSocialMedia()
    {
        return new List<string>(socialMediaPosts);
    }

    void TriggerVisualEffect(string effectName)
    {
        // Visual effects will be handled by the UI manager
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowVisualEffect(effectName);
        }
    }

    void PlayAudioEffect(string clipName)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            // Audio clips would be loaded from Resources
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + clipName);
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }

    public void EndGame()
    {
        string endingType = resources.GetEndingType();
        PlayerPrefs.SetString("EndingType", endingType);
        PlayerPrefs.SetInt("FinalDecisions", decisionsCount);
        PlayerPrefs.SetFloat("FinalPopularity", resources.popularity);
        PlayerPrefs.SetFloat("FinalStability", resources.stability);
        PlayerPrefs.SetFloat("FinalMedia", resources.media);
        PlayerPrefs.SetFloat("FinalEconomic", resources.economic);
        
        // Load ending scene or show ending UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowEnding(endingType);
        }
    }

    public void StartNewGame()
    {
        resources = new GameResources();
        decisionsCount = 0;
        newsHeadlines.Clear();
        socialMediaPosts.Clear();
        ShuffleDeck();
        SceneManager.LoadScene("Crisis");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
