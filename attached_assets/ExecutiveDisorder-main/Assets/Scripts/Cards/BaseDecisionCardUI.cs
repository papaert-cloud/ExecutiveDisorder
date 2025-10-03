using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseDecisionCardUI : MonoBehaviour
{
    [Header("Card Data")]
    [Tooltip("The card data presented on the ui")]
    [SerializeField]
    protected DecisionCard m_DecisionCardData;

    [Header("Card info")]
    [SerializeField]
    protected TextMeshProUGUI m_CardTitle;

    [SerializeField]
    protected TextMeshProUGUI m_Description;

    [Header("Card containers")]
    [SerializeField]
    protected Transform m_ResponsesContainer;

    [Header("Card Prefabs")]

    [SerializeField]
    protected GameObject m_DecisionPrefab;

    [Header("Card Events")]
    public Action<int> OnDecisionSelected;

    public Action OnAnimationFinished;

    public virtual void SetDecisionCardData(DecisionCard decisionCard)
    {
        m_DecisionCardData = decisionCard;
        UpdateCardUI();
    }

    protected virtual void UpdateCardUI()
    {
    }

    protected virtual void UpdateResponseItems()
    {
        foreach (Transform responseTransform in m_ResponsesContainer)
        {
            responseTransform.GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(responseTransform.gameObject);
        }

        for (int i = 0; i < 4 && i < m_DecisionCardData.MultipleChoiceOptions.Count; i++)
        {

            bool IsDelay = m_DecisionCardData.MultipleChoiceOptions[i].IsDelayOption;
            bool hasBeenDelayed = HistoryManager.Instance.HasBeenDelayed(m_DecisionCardData.CardID);

            if (hasBeenDelayed && IsDelay) { continue; }

            GameObject decisionResponseObject = Instantiate(m_DecisionPrefab, m_ResponsesContainer);

            int savedIndex = i;
            decisionResponseObject.GetComponent<Button>().onClick.AddListener(() => OnResponseClick(savedIndex));

            decisionResponseObject.GetComponent<CardResponseItemUI>().PointerEnter += (() => OnPointerEnter(savedIndex));
            decisionResponseObject.GetComponent<CardResponseItemUI>().PointerExit += (() => OnPointerExit(savedIndex));

            CardResponseItemUI decisionResponseItem = decisionResponseObject.GetComponent<CardResponseItemUI>();
            if (decisionResponseItem == null)
            {
                Debug.LogWarning("ResponsObject doesnt have the responseitem class");
                return;

            }

            bool HasConsequence = m_DecisionCardData.MultipleChoiceOptions[i].ConsequenceCardID >= 0;

            string decisionText = m_DecisionCardData.MultipleChoiceOptions[i].OptionText;
            List<ResourceCost> costList = m_DecisionCardData.MultipleChoiceOptions[i].ResourceRequirements;

            decisionResponseItem.SetDecision(decisionText, costList, hasBeenDelayed, IsDelay, HasConsequence);
        }
    }

    protected virtual void OnResponseClick(int index)
    {
        OnDecisionSelected?.Invoke(index);
        DecisionOption option = DecisionsManager.Instance.CurrentDecisionCard.MultipleChoiceOptions[index];
        NewsHeadlineManager.Instance.SetHeadline(option.NewsHeadline);
        PlayEndAnimation();
    }

    protected virtual void PlayEndAnimation()
    {

    }

    protected virtual void OnPointerEnter(int index)
    {
        Dictionary<ResourceType, float> costByType = m_DecisionCardData.MultipleChoiceOptions[index].GetResourcesAffected();

        bool hasBeenDelayed = HistoryManager.Instance.HasBeenDelayed(m_DecisionCardData.CardID);

        ResourcesManager.Instance.PreviewResources(costByType, hasBeenDelayed);
    }

    protected virtual void OnPointerExit(int index)
    {
        ResourcesManager.Instance.StopResourcePreview();
    }

    protected virtual void AnimationFinish()
    {
        OnAnimationFinished?.Invoke();
    }

    public virtual void CardExit()
    {

    }

    public virtual void UpdateTimer(float timeLeft)
    {
    }

    public virtual void SetTimerVisbility(bool HasTime)
    {

    }
}
