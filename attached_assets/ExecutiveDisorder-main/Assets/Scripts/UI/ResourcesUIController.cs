using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;

public class ResourcesUIController : MonoBehaviour
{
    [SerializeField] ResourceUI m_popularityResourceUI;
    [SerializeField] ResourceUI m_stabilityResourceUI;
    [SerializeField] ResourceUI m_mediaResourceUI;
    [SerializeField] ResourceUI m_economicResourceUI;

    private int m_hidesCompleted;

    private void OnEnable()
    {
        ResourcesManager.Instance.OnResourceChanged += UpdateResourceUI;
        ResourcesManager.Instance.ResourcePreviewRequest += PreviewResources;
        ResourcesManager.Instance.StopResourcePreviewRequest += StopResourcePreview;
        ResourcesManager.Instance.StopPreviewOnDecisionRequest += StopPreviewOnDecision;
        InitializeValues();
    }

    private void OnDisable()
    {
        ResourcesManager.Instance.OnResourceChanged -= UpdateResourceUI;
        ResourcesManager.Instance.ResourcePreviewRequest -= PreviewResources;
        ResourcesManager.Instance.StopResourcePreviewRequest -= StopResourcePreview;
        ResourcesManager.Instance.StopPreviewOnDecisionRequest -= StopPreviewOnDecision;
    }

    private void InitializeValues()
    {
        for (int i = 0; i < 4; i++)
        {
            UpdateResource((ResourceType)i, ResourcesManager.Instance.GetResource((ResourceType)i));
        }
    }

    private void UpdateResourceUI(ResourceType resourceType, float resourceAmount)
    {
        UpdateResource(resourceType, resourceAmount);
    }

    private void PreviewResources(Dictionary<ResourceType, float> resources, bool showValue)
    {
        foreach (ResourceType resource in resources.Keys)
        {
            float resourceAmount = resources[resource];
            PreviewResource(resource, resourceAmount, showValue);
        }
    }

    private void UpdateResource(ResourceType resourceType, float amount)
    {
        switch (resourceType)
        {
            case ResourceType.Popularity:
                m_popularityResourceUI.UpdateResourceText("Popularity", amount);
                break;
            case ResourceType.Stability:
                m_stabilityResourceUI.UpdateResourceText("Stability", amount);
                break;
            case ResourceType.Media:
                m_mediaResourceUI.UpdateResourceText("Media Trust",amount);
                break;
            case ResourceType.Economic:
                m_economicResourceUI.UpdateResourceText("Economic", amount);
                break;
        }
    }

    private void PreviewResource(ResourceType resourceType, float amount, bool showValue)
    {
        if (amount == 0) return;

        switch (resourceType)
        {
            case ResourceType.Popularity:
                m_popularityResourceUI.ShowResourcePreview(amount, showValue);
                break;
            case ResourceType.Stability:
                m_stabilityResourceUI.ShowResourcePreview(amount, showValue);
                break;
            case ResourceType.Media:
                m_mediaResourceUI.ShowResourcePreview(amount, showValue); 
                break;
            case ResourceType.Economic:
                m_economicResourceUI.ShowResourcePreview(amount, showValue);
                break;
        }
    }

    private void StopPreviewOnDecision()
    {
        m_hidesCompleted = 0;
        m_popularityResourceUI.OnHideCompleted += PreviewHideCompleted;
        m_stabilityResourceUI.OnHideCompleted += PreviewHideCompleted;
        m_mediaResourceUI.OnHideCompleted += PreviewHideCompleted;
        m_economicResourceUI.OnHideCompleted += PreviewHideCompleted;

        StopResourcePreview();

    }

    private void PreviewHideCompleted()
    {
        m_hidesCompleted++;
        if(m_hidesCompleted >= 4) 
        {
            ResourcesManager.Instance.ResourcesHideCompleted();
            m_popularityResourceUI.OnHideCompleted -= PreviewHideCompleted;
            m_stabilityResourceUI.OnHideCompleted -= PreviewHideCompleted;
            m_mediaResourceUI.OnHideCompleted -= PreviewHideCompleted;
            m_economicResourceUI.OnHideCompleted -= PreviewHideCompleted;
        }
    }

    private void StopResourcePreview()
    {
        m_popularityResourceUI.HideResourcePreview();
        m_stabilityResourceUI.HideResourcePreview();
        m_mediaResourceUI.HideResourcePreview();
        m_economicResourceUI.HideResourcePreview();
    }
}
