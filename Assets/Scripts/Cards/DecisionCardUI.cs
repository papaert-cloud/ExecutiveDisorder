using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DecisionCardUI : BaseDecisionCardUI
{
    [Header("Card info")]
    [SerializeField]
    private TextMeshProUGUI m_CardCategory;

    [SerializeField]
    private TextMeshProUGUI m_CardUrgency;

    [Header("Card containers")]
    [SerializeField]
    private Transform m_StakeHoldersContainer;

    [Header("Card Prefabs")]
    [SerializeField]
    private GameObject m_StakeHolderPrefab;

    private void Start()
    {
        GetComponent<UIMoveAnimation>().OnOutAnimComplete += AnimationFinish;
    }

    public override void SetDecisionCardData(DecisionCard decisionCard)
    {
        base.SetDecisionCardData(decisionCard);
        GetComponent<UIMoveAnimation>().PlayCtoA();
    }


    protected override void UpdateCardUI()
    {
        m_CardTitle.text = m_DecisionCardData.Title;
        m_CardCategory.text = GameAssetsManager.Instance.GetCategoryDisplayName(m_DecisionCardData.Category);
        m_CardUrgency.text = GameAssetsManager.Instance.GetCategoryUrgencyName(m_DecisionCardData.Urgency);
        m_Description.text = m_DecisionCardData.Description;

        UpdateStakeHoldersItems();

        UpdateResponseItems();
    }

    protected override void PlayEndAnimation()
    {
        base.PlayEndAnimation();
        GetComponent<UIMoveAnimation>().PlayAtoB();
    }

    protected override void OnResponseClick(int index)
    {
        CardExit();
        DecisionOption option = DecisionsManager.Instance.CurrentDecisionCard.MultipleChoiceOptions[index];
        PlayDecisionReaction(option);
        StartCoroutine(BaseResponseClick(index));
    }
     
    private IEnumerator BaseResponseClick(int index)
    {
        yield return new WaitForSeconds(0.1f);
        base.OnResponseClick(index);
    }

    private void UpdateStakeHoldersItems()
    {

    }

    public override void CardExit()
    {
        base.CardExit();
        AudioManager.Instance.PlaySFX(AudioManager.SoundType.Stamp, 1, 1.8f);
    }

    private void PlayDecisionReaction(DecisionOption option)
    {
        float totalValue = 0f;
        foreach (var kvp in option.ResourceRequirements)
        {
            totalValue += kvp.Amount;
        }

        if (totalValue >= 0f) AudioManager.Instance.PlaySFXForRandomPitch(AudioManager.SoundType.Applause, 1f, 0.8f, 1.2f, 0.6f, 1.2f);
        else AudioManager.Instance.PlaySFXForRandomPitch(AudioManager.SoundType.Boo, 1f, 1f, 1.2f, 0f, 1.8f);
        if (totalValue >= 0f) AudioManager.Instance.PlaySFXForRandomPitch(AudioManager.SoundType.Applause, 1f, 0.8f, 1.2f, 0.4f, 1.2f);
    }
}
