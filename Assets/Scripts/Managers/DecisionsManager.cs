using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class DecisionsManager : MonoBehaviour
{
    public static DecisionsManager Instance { get; private set; }

    [Header("Cards UI")]
    [SerializeField]
    List<DecisionCardUIData> m_decisionsUIData = new List<DecisionCardUIData>();

    Dictionary<CardCategory, BaseDecisionCardUI> m_decisionUIMap = new Dictionary<CardCategory, BaseDecisionCardUI>();

    [SerializeField] private BaseDecisionCardUI m_currentDecisionCardUI;

    [Header("Cards Data")]
    [SerializeField] private List<DecisionCard> m_AllCardsData = new List<DecisionCard>();

    private Dictionary<int, DecisionCard> m_AllCardsMap = new Dictionary<int, DecisionCard>();
    private Dictionary<int, DecisionCard> m_DecisionCardsDataMap = new Dictionary<int, DecisionCard>();
    private Dictionary<int, DecisionCard> m_CrisisCardsDataMap = new Dictionary<int, DecisionCard>();

    [Header("Decisions Params")]
    [SerializeField] private int m_DecisionPerRound;

    public DecisionCard CurrentDecisionCard => m_CurrentDecisionCard;

    private DecisionCard m_CurrentDecisionCard;
    private int m_DecisionsTaken;

    [SerializeField]
    private int m_DecisionsSinceLastCrisis;
    [SerializeField]
    AnimationCurve m_AnimationCurve;

    [SerializeField]
    private List<DecisionCard> m_QueuedDecisionCards = new List<DecisionCard>();

    [Header("Decisions Events")]
    public Action<DecisionOption> OnDecisionSelected;

    private float m_CrisisTimeRemaining;
    [SerializeField]
    private bool m_CrisisTimerActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate DecisionsManager detected. Destroying the new one.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            Init();
        }
    }

    private void Update()
    {
        if (m_CrisisTimerActive)
        {
            m_CrisisTimeRemaining -= Time.deltaTime;
            GameObject substateUI = StateManager.Instance.GetCurrentSubstateUI();
            BaseDecisionCardUI baseDecisionCardUI = null;
            if (substateUI)
            {
                baseDecisionCardUI = substateUI.GetComponent<BaseDecisionCardUI>();
                baseDecisionCardUI.UpdateTimer(Mathf.Clamp(m_CrisisTimeRemaining, 0, m_CurrentDecisionCard.TimeLimitSeconds)/m_CurrentDecisionCard.TimeLimitSeconds);
            }

            if (m_CrisisTimeRemaining <= 0f)
            {
                m_CrisisTimerActive = false;
                if (substateUI)
                {
                    baseDecisionCardUI = substateUI.GetComponent<BaseDecisionCardUI>();
                    baseDecisionCardUI.CardExit();

                }
                OnDecisionCardOut();
            }
        }
    }

    private void Init()
    {
        foreach (DecisionCard card in m_AllCardsData)
        {
            if (card != null)
            {
                // Use CardID as key
                m_AllCardsMap.Add(card.CardID, card);
                if (card.Category != CardCategory.Crisis)
                {
                    if (!card.IsConsequence) m_DecisionCardsDataMap.Add(card.CardID, card);
                } else
                {
                    m_CrisisCardsDataMap.Add(card.CardID, card);
                }
            }
        }
    }

    private void Start()
    {
        if (m_decisionsUIData.Count <= 0)
        {
            Debug.LogError("DecisionCardUI is not set on the DecisionManger cannot be initialized");
            return;
        }

        foreach(DecisionCardUIData decisionUIData in m_decisionsUIData)
        {
            decisionUIData.BaseDecisionCardUI.OnDecisionSelected += OnDecisionUISelect;
            decisionUIData.BaseDecisionCardUI.OnAnimationFinished += OnDecisionCardOut;
        }

        m_decisionUIMap = new Dictionary<CardCategory, BaseDecisionCardUI>();

        foreach (var data in m_decisionsUIData)
        {
            foreach (var category in data.Categories)
            {
                if (!m_decisionUIMap.ContainsKey(category))
                {
                    m_decisionUIMap.Add(category, data.BaseDecisionCardUI);
                }
                else
                {
                    Debug.LogWarning($"Duplicate mapping for category {category} detected. Skipping.");
                }
            }
        }

    }

    public DecisionCard GetDecisionCardById(int id)
    {
        if (m_AllCardsMap.ContainsKey(id))
        {
            if (m_DecisionCardsDataMap.ContainsKey(id))
            {
                m_DecisionCardsDataMap.Remove(id);
            }
            return m_AllCardsMap[id];
        }
        return null;
    }

    public void RequestDecision(DecisionCard decisionCard)
    {
        m_currentDecisionCardUI = GetDecisionCardUI(decisionCard.Category);

        if (m_currentDecisionCardUI == null)
        {
            Debug.LogError("currentDecisionCardUI is not set on the DecisionManger cannot request a decision");
            return;
        }

        m_CurrentDecisionCard = decisionCard;
        m_currentDecisionCardUI.SetDecisionCardData(m_CurrentDecisionCard);
    }

    private void OnDecisionUISelect(int decisionIndex)
    {
        DecisionSelect(decisionIndex);
    }

    public DecisionCard GetcurrentDecisionCard()
    {
        return m_CurrentDecisionCard;
    }

    private void DecisionSelect(int decisionIndex)
    {
        OnDecisionSelected?.Invoke(m_CurrentDecisionCard.MultipleChoiceOptions[decisionIndex]);
    }

    private void OnDecisionCardOut()
    {
        m_DecisionsTaken++;

        if (HasFinishedRound())
        {
            ////Extra Decision 
            //SortQueuedDecisionsByUrgency();
            //DecisionCard queuedCard = m_QueuedDecisionCards[0];
            //if (queuedCard.Category == CardCategory.Crisis && m_DecisionsTaken < m_DecisionPerRound) 
            //{ 
            //    NextCard(); 
            //    return;
            //};

            ExitCardUIState(m_CurrentDecisionCard);
            return;
        }

        NextCard();
    }

    private bool HasFinishedRound()
    {
        return m_DecisionsTaken >= m_DecisionPerRound;
    }

    public void StartDecisions()
    {
        if (HasFinishedRound() || m_DecisionsTaken == 0 && m_CurrentDecisionCard == null) StartRound();
        else ContinueRound();
    }

    private void ContinueRound()
    {
        EnterCardUIState(m_CurrentDecisionCard);
    }

    public void PauseRound()
    {
        ExitCardUIState(m_CurrentDecisionCard);
        AudioManager.Instance.PlayMusic(AudioManager.SoundType.RelaxingPark);
    }

    private void StartRound()
    {
        m_DecisionsTaken = 0;
        NextCard();
    }

    public void NextCard()
    {
        DecisionCard nextCard = GetDecisionCard();
        RequestCard(nextCard);
    }

    public void RequestCard(DecisionCard card)
    {
        if (card == null) return;
        EnterCardUIState(card);
        RequestDecision(card);
    }

    private void EnterCardUIState(DecisionCard card)
    {
        m_CrisisTimerActive = false;
        switch (card.Category)
        {
            case CardCategory.Policy:
            case CardCategory.SocialMedia:
            case CardCategory.Opportunity:
            case CardCategory.Character:
            case CardCategory.Legacy:
                if (!StateManager.Instance.ActiveSubStates.Contains(SubAppState.CardDecision))
                {
                    StateManager.Instance.EnterSubState(SubAppState.CardDecision);
                }
                break;
            case CardCategory.Crisis:
                if (!StateManager.Instance.ActiveSubStates.Contains(SubAppState.CrisisDecision))
                {
                    StateManager.Instance.EnterSubState(SubAppState.CrisisDecision);
                }


                if(card.TimeLimitSeconds > 4)
                {
                    // start crisis timer
                    m_CrisisTimeRemaining = card.TimeLimitSeconds;
                    m_CrisisTimerActive = true;
                    GameObject CurrentUI = StateManager.Instance.GetCurrentSubstateUI();
                    CurrentUI.GetComponent<BaseDecisionCardUI>().SetTimerVisbility(true);
                } else
                {
                    GameObject CurrentUI = StateManager.Instance.GetCurrentSubstateUI();
                    CurrentUI.GetComponent<BaseDecisionCardUI>().SetTimerVisbility(false);
                }

                break;
        }
    }

    private void ExitCardUIState(DecisionCard card)
    {
        switch (card.Category)
        {
            case CardCategory.Policy:
            case CardCategory.SocialMedia:
            case CardCategory.Opportunity:
            case CardCategory.Character:
            case CardCategory.Legacy:
                if (StateManager.Instance.ActiveSubStates.Contains(SubAppState.CardDecision))
                {
                    StateManager.Instance.ExitSubState(SubAppState.CardDecision);
                }
                break;
            case CardCategory.Crisis:
                if (StateManager.Instance.ActiveSubStates.Contains(SubAppState.CrisisDecision))
                {
                    StateManager.Instance.ExitSubState(SubAppState.CrisisDecision);
                }
                break;
        }

        TryEnterMenuActionsState();
    }

    private void TryEnterMenuActionsState()
    {
        if (StateManager.Instance.ActiveState == AppState.MainMenu &&
            !StateManager.Instance.ActiveSubStates.Contains(SubAppState.MenuActions))
        {
            StateManager.Instance.EnterSubState(SubAppState.MenuActions);
        }
    }

    private DecisionCard GetDecisionCard()
    {
        SortQueuedDecisionsByUrgency();

        // Evaluate all queued cards in order
        for (int i = 0; i < m_QueuedDecisionCards.Count; i++)
        {
            DecisionCard queuedCard = m_QueuedDecisionCards[i];
            float chance = GetChanceFromUrgency(queuedCard.Urgency);
            float roll = UnityEngine.Random.Range(0f, 1f);

            if (queuedCard.Urgency == UrgencyLevel.Immediate || roll <= chance)
            {
                if (queuedCard.Category == CardCategory.Crisis)
                {
                    m_DecisionsSinceLastCrisis = 0;
                    m_CrisisCardsDataMap.Remove(queuedCard.CardID);
                } else
                {
                    m_DecisionsSinceLastCrisis++;
                }
                m_QueuedDecisionCards.RemoveAt(i);
                return queuedCard;
            }
        }

        // Fallback to random decision card
        if (m_DecisionCardsDataMap.Count <= 0)
        {
            Debug.LogWarning("No more decision cards available.");
            return null;
        }

        float crisisRoll = UnityEngine.Random.Range(0f, 1f);
        float crisisChance = Mathf.Clamp(m_AnimationCurve.Evaluate(m_DecisionsSinceLastCrisis),0f,1f);

        if(crisisRoll <= crisisChance)
        {
            m_DecisionsSinceLastCrisis = 0;

            // Get all values (DecisionCards) from the dictionary
            var crisisValues = m_CrisisCardsDataMap.Values.ToList();

            int crisisRandomIndex = UnityEngine.Random.Range(0, crisisValues.Count);

            // Pick a random one
            DecisionCard outRandomCrisis = crisisValues[crisisRandomIndex];

            m_CrisisCardsDataMap.Remove(outRandomCrisis.CardID);
            return outRandomCrisis;
        }

        // Get all values (DecisionCards) from the dictionary
        var values = m_DecisionCardsDataMap.Values.ToList();

        int randomIndex = UnityEngine.Random.Range(0, values.Count);

        // Pick a random one
        DecisionCard outRandomCard = values[randomIndex];

        if(outRandomCard.Category != CardCategory.Crisis)
        {
            m_DecisionsSinceLastCrisis++;
        }

        m_DecisionCardsDataMap.Remove(outRandomCard.CardID);
        return outRandomCard;
    }
    private void SortQueuedDecisionsByUrgency()
    {
        // Higher urgency (Immediate) comes before lower urgency (Low)
        m_QueuedDecisionCards.Sort((a, b) => b.Urgency.CompareTo(a.Urgency));
    }

    private float GetChanceFromUrgency(UrgencyLevel urgency)
    {
        switch (urgency)
        {
            case UrgencyLevel.High: return 0.7f;
            case UrgencyLevel.Medium: return 0.45f;
            case UrgencyLevel.Low: return 0.3f;
            default: return 0f;
        }
    }

    private void QueueDecisionCard(DecisionCard card)
    {
        m_QueuedDecisionCards.Add(card);
    }

    public void QueueDecisionCard(int id)
    {
        DecisionCard decisionCardToQueue = GetDecisionCardById(id);
        if (decisionCardToQueue)
        {
            QueueDecisionCard(decisionCardToQueue);
        }
    }

    public BaseDecisionCardUI GetDecisionCardUI(CardCategory category)
    {
        BaseDecisionCardUI outDecisionUI = null;
        m_decisionUIMap.TryGetValue(category, out outDecisionUI);
        return outDecisionUI;
    }
}

[System.Serializable]
public struct DecisionCardUIData
{
    public BaseDecisionCardUI BaseDecisionCardUI;
    public List<CardCategory> Categories;
}
