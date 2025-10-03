using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeMePostUI : MonoBehaviour
{
    [SerializeField] private Image m_profilePicture;
    [SerializeField] private TextMeshProUGUI m_username;
    [SerializeField] private TextMeshProUGUI m_postText;
    [SerializeField] private List<EmojiReactUI> m_negativeReacts = new List<EmojiReactUI>();
    [SerializeField] private List<EmojiReactUI> m_positiveReacts = new List<EmojiReactUI>();

    [SerializeField] private GameObject m_resourcePreviewPrefab;
    [SerializeField] private Transform m_resourcePreviewHolder;

    private MediaReaction m_currentReaction;

    public Action<MediaReaction> OnReactionTaken;

    private void Start()
    {
        foreach (var emoji in m_negativeReacts)
        {
            emoji.PointerEnter += HoverNegative;

            Button button = emoji.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(ReactNegative);
        }

        foreach (var emoji in m_positiveReacts)
        {
            emoji.PointerEnter += HoverPositive;

            Button button = emoji.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(ReactPositive);
        }
    }

    public void Animate()
    {
        RectTransform rect = GetComponent<RectTransform>();

        // Force layout rebuild to get correct anchoredPosition
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)rect.parent);

        // Cache the final layout position
        Vector2 targetPos = rect.anchoredPosition;

        // Start above the position
        rect.anchoredPosition = targetPos + new Vector2(0, 200f);

        // Animate into place
        rect.DOAnchorPos(targetPos, 0.4f).SetEase(Ease.OutCubic);

    }

    public void SetMeMePost(MediaReaction mediaReaction)
    {
        m_currentReaction = mediaReaction;
        m_postText.text = mediaReaction.Text;
        m_username.text = mediaReaction.SourceName;
        CreateResourcesAffected();
    }

    private void CreateResourcesAffected()
    {
        Dictionary<ResourceType, float> resources = m_currentReaction.GetResourcesAffected();

        foreach (var resource in resources)
        {
            if (resource.Value == 0) continue;
            GameObject politicObject = Instantiate(m_resourcePreviewPrefab, m_resourcePreviewHolder);
            politicObject.GetComponent<MediaResourceUI>().SetResourceUI(resource.Key, resource.Value.ToString());

        }
    }

    public void HoverNegative()
    {
        Debug.Log("Hovering over negative emoji react preview");
    }

    public void HoverPositive()
    {
        Debug.Log("Hovering over positive emoji react preview");
    }

    public void ReactPositive()
    {
        DisableReactionsInteractable();
        ResourcesManager.Instance.AddResources(m_currentReaction.GetResourcesAffected());
        OnReactionTaken?.Invoke(m_currentReaction);
    }

    public void ReactNegative()
    {
        DisableReactionsInteractable();
        ResourcesManager.Instance.AddResources(m_currentReaction.GetResourcesAffected(true));
        OnReactionTaken?.Invoke(m_currentReaction);
    }

    private void DisableReactionsInteractable()
    {
        foreach (var emoji in m_negativeReacts)
        {
            Button button = emoji.GetComponent<Button>();
            if (button != null)
                button.interactable = false;
        }

        foreach (var emoji in m_positiveReacts)
        {
            Button button = emoji.GetComponent<Button>();
            if (button != null)
                button.interactable = false;
        }
    }
}
