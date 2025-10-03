using System;
using System.Collections.Generic;
using UnityEngine;

public class MeMediaUI : MonoBehaviour
{
    [SerializeField] private Transform m_memePostsHolder;
    [SerializeField] private GameObject m_memePostPrefab;
    [SerializeField] private GameObject m_memePostSeparatorPrefab;

    private DecisionCard m_decisionCard;

    public Action<MediaReaction> OnReactionTaken;

    private void Start()
    {
        DecisionsManager.Instance.OnDecisionSelected += OnDecisionSelected;
    }

    private void OnDecisionSelected(DecisionOption decisionOption)
    {
        m_decisionCard = DecisionsManager.Instance.CurrentDecisionCard;
        foreach(MediaReaction mediaReaction in m_decisionCard.GetMediaReactionsByType(decisionOption.ReactionType))
        {
            CreateMemePost(mediaReaction);
        }
    }

    public void CreateMemePost(MediaReaction mediaReaction)
    {
        GameObject memePost = Instantiate(m_memePostPrefab, m_memePostsHolder);
        MeMePostUI meMePostUI = memePost.GetComponent<MeMePostUI>();
        meMePostUI.OnReactionTaken += OnReactionTakenMeMePost;
        meMePostUI.SetMeMePost(mediaReaction);
        GameObject memePostSeparator = Instantiate(m_memePostSeparatorPrefab, m_memePostsHolder);
    }

    public void GoToMainMenu()
    {
        StateManager.Instance.SwitchState(AppState.MainMenu);
    }
    
    private void OnReactionTakenMeMePost(MediaReaction mediaReaction)
    {
        OnReactionTaken?.Invoke(mediaReaction);
    }
}
