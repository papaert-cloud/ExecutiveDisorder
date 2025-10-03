using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HistoryManager : MonoBehaviour
{
    public static HistoryManager Instance;

    [SerializeField]
    List<DecisionCard> m_takenDecisionsCard;
    [SerializeField]
    List<DecisionOption> m_takenDecisionsResponse;
    [SerializeField]
    List<DecisionCard> m_delayedDecisionCards;
    [SerializeField]
    List<MediaReaction> m_reactions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate HistoryManager detected. Destroying the new one.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DecisionsManager.Instance.OnDecisionSelected += OnDecisionSelected;
        NewsManager.Instance.OnReactionTaken += OnReactionTaken;
    }

    private void OnDecisionSelected(DecisionOption Decision) 
    {
        DecisionCard currentDecisionCard = DecisionsManager.Instance.CurrentDecisionCard;

        m_takenDecisionsCard.Add(currentDecisionCard);
        m_takenDecisionsResponse.Add(Decision);

        if (Decision.ConsequenceCardID >= 0)
        {
            DecisionsManager.Instance.QueueDecisionCard(Decision.ConsequenceCardID);
        } 
        else if (Decision.IsDelayOption)
        {
            m_delayedDecisionCards.Add(currentDecisionCard);
            DecisionsManager.Instance.QueueDecisionCard(currentDecisionCard.CardID);
        }
    }

    private void OnReactionTaken(MediaReaction reaction)
    {
        m_reactions.Add(reaction);  
    }

    public bool HasBeenDelayed(int CardID)
    {
        return m_delayedDecisionCards.Any(card => card.CardID == CardID);
    }

}
