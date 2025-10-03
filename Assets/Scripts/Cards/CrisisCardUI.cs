using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrisisCardUI : BaseDecisionCardUI
{

    [Header("Card info")]
    [SerializeField]
    private TextMeshProUGUI m_CardTitleSecond;

    [SerializeField]
    private Image m_CardImage;

    [Header("Card containers")]
    [SerializeField]
    private Transform m_StakeHoldersContainer;

    [Header("Card Prefabs")]
    [SerializeField]
    private GameObject m_StakeHolderPrefab;

    [SerializeField]
    private NewsTicker m_FirstTitleTicker;
    [SerializeField]
    private NewsTicker m_SecondTitleTicker;
    [SerializeField]
    private NewsTicker m_DescriptionTicker;

    [SerializeField]
    private TVPowerEffect m_powerEffect;

    [SerializeField]
    private CircularSlider m_CircularSlider;

    private void Start()
    {
        m_powerEffect.OnAnimationFinish += AnimationFinish;
    }

    public override void SetDecisionCardData(DecisionCard decisionCard)
    {
        AudioManager.Instance.PlaySFXAtTime(AudioManager.SoundType.TvStatic, 0.2f);
        AudioManager.Instance.PlaySFX(AudioManager.SoundType.News);
        base.SetDecisionCardData(decisionCard);
    }

    protected override void UpdateCardUI()
    {
        m_CardTitle.text = m_DecisionCardData.Title;
        m_CardTitleSecond.text = m_DecisionCardData.Title;
        m_Description.text = m_DecisionCardData.Description;
        m_DescriptionTicker.StartScrolling();
        if (m_DecisionCardData.CardSprite)
            m_CardImage.sprite = m_DecisionCardData.CardSprite;

        if (m_powerEffect.isOn) m_powerEffect.ToggleTV();

        //UpdateStakeHoldersItems();

        UpdateResponseItems();
    }

    protected override void OnResponseClick(int index)
    {
        CardExit();
        DecisionOption option = DecisionsManager.Instance.CurrentDecisionCard.MultipleChoiceOptions[index];
        PlayDecisionReaction(option);
        StartCoroutine(BaseResponseClick(index));
        NewsHeadlineManager.Instance.SetHeadline(option.NewsHeadline);
    }

    private IEnumerator BaseResponseClick(int index)
    {
        yield return new WaitForSeconds(0.1f);
        if (!m_powerEffect.isOn) m_powerEffect.ToggleTV();
        OnDecisionSelected?.Invoke(index);
    }

    private void OnDisable()
    {
        m_FirstTitleTicker.StopScrolling();
        m_SecondTitleTicker.StopScrolling();
    }

    public override void CardExit()
    {
        base.CardExit();
        AudioManager.Instance.PlaySFX(AudioManager.SoundType.TvOff);
        AudioManager.Instance.StopSFX(AudioManager.SoundType.News);
    }

    private void PlayDecisionReaction(DecisionOption option)
    {
        float totalValue = 0f;
        foreach (var kvp in option.ResourceRequirements)
        {
            totalValue += kvp.Amount;
        }

        if(totalValue >= 0f)
        {
            AudioManager.Instance.PlaySFXForRandomPitch(AudioManager.SoundType.Applause, 1f, 0.8f, 1.2f, 0.6f, 1.2f);
            AudioManager.Instance.PlaySFXForRandomPitch(AudioManager.SoundType.Applause, 1f, 0.8f, 1.2f, 0.4f, 1.2f);

            AudioManager.Instance.PlaySFXForRandomPitch(AudioManager.SoundType.Pop, 1f, 0.8f, 1.2f, 0f, 1.2f);
            AudioManager.Instance.PlaySFXForRandomPitch(AudioManager.SoundType.Confetti, 2f, 0.8f, 1.2f, 0.4f, 1.2f);

            EffectManager.Instance.PlayVFX(VFXType.Confeti);

            ReactionsManager.Instance.PlayPositiveReaction();
        } 
        else
        {
            AudioManager.Instance.PlaySFXForRandomPitch(AudioManager.SoundType.Boo, 1f, 1f, 1.2f, 0f, 1.8f);
            AudioManager.Instance.PlaySFXForRandomPitch(AudioManager.SoundType.Incorrect, 1f, 0.8f, 1.2f, 0f, 1.2f);

            EffectManager.Instance.PlayUIVFX(VFXType.RedFlash);
            ReactionsManager.Instance.PlayNegativeReaction();


        }
    }

    public override void UpdateTimer(float timeLeft)
    {
        base.UpdateTimer(timeLeft);
        m_CircularSlider.value = timeLeft;
        m_CircularSlider.UpdateVisuals();

        if (timeLeft <= 0f) 
        {
            AudioManager.Instance.PlaySFXFor(AudioManager.SoundType.ClockOff, 2f, 1f);
        }
    }

    public override void SetTimerVisbility(bool HasTime)
    {
        base.SetTimerVisbility(HasTime);
        m_CircularSlider.gameObject.SetActive(HasTime);
    }
}
