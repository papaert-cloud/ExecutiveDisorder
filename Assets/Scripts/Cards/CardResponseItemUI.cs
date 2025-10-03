using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardResponseItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action PointerEnter;
    public Action PointerExit;

    [SerializeField] TextMeshProUGUI m_DecisionText;
    [SerializeField] Transform m_ResourcesInfoContainer;
    [SerializeField] GameObject m_ResourcePrefab;
    [SerializeField] Button m_DecisionButton;
    [SerializeField] GameObject m_IsDelayObject;
    [SerializeField] GameObject m_IsConsequenceObject;

    private void Start()
    {
        DecisionsManager.Instance.OnDecisionSelected += OnDecisionSelected;
    }

    public void SetDecision(string decisionText, List<ResourceCost> resourceCosts, bool showValue, bool IsDelay, bool IsConsequence)
    {
        m_IsDelayObject.SetActive(IsDelay);
        m_IsConsequenceObject.SetActive(IsConsequence);

        m_DecisionText.text = decisionText;

        Dictionary<ResourceType, float> costByType = new Dictionary<ResourceType, float>
        {
            { ResourceType.Popularity, 0f },
            { ResourceType.Stability, 0f },
            { ResourceType.Media, 0f },
            { ResourceType.Economic, 0f }
        };

        foreach (var resourceCost in resourceCosts)
        {
            if (costByType.ContainsKey(resourceCost.Type))
                costByType[resourceCost.Type] += resourceCost.Amount;
        }

        ApplyCost(ResourceType.Popularity, costByType[ResourceType.Popularity], showValue);
        ApplyCost(ResourceType.Stability, costByType[ResourceType.Stability], showValue);
        ApplyCost(ResourceType.Media, costByType[ResourceType.Media], showValue);
        ApplyCost(ResourceType.Economic, costByType[ResourceType.Economic], showValue);
    }

    private void ApplyCost(ResourceType type, float amount, bool showValue = false)
    {
        if (amount == 0) return;

        Sprite resourceSprite = GameAssetsManager.Instance.GetResourceIcon(type);

        GameObject resourceObject = Instantiate(m_ResourcePrefab, m_ResourcesInfoContainer);

        string prefix = (amount > 0) ? "+" : "";
        string resourceTextPreview = (showValue) ? $"{prefix}{amount}" : "?";

        ResourceIconUI resourceIconUI = resourceObject.GetComponent<ResourceIconUI>();
        resourceIconUI.SetResource(resourceSprite, resourceTextPreview);

        Color previewColor = (amount > 0) ? Color.green : Color.red;    
        resourceIconUI.EnablePreviewIcon(previewColor);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_DecisionButton.IsInteractable())
            PointerEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_DecisionButton.IsInteractable())
            PointerExit?.Invoke();
    }

    private void OnDecisionSelected(DecisionOption decisionOption)
    {
        m_DecisionButton.interactable = false;
    }

}
