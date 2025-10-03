using System.Collections.Generic;
using UnityEngine;

public class GameAssetsManager : MonoBehaviour
{
    public static GameAssetsManager Instance { get; private set; }

    public List<SpriteEntries> SpritesEntries;
    private Dictionary<SpriteEntryKey, Sprite> m_spriteEntriesDict;

    [Header("Card Const")]
    [SerializeField]
    private List<CardCategoryDisplayName> m_CategoryDisplayNames;

    [SerializeField]
    private List<CardCUrgencyDisplayName> m_UrgencyDisplayNames;

    [SerializeField]
    private List<ResourceTypeDisplayName> m_ResourceTypeDisplayNames;

    private Dictionary<CardCategory, string> m_CategoryDisplayNameMap;
    private Dictionary<ResourceType, string> m_ResourceDisplayNameMap;
    private Dictionary<UrgencyLevel, string> m_UrgencyDisplayNameMap;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            m_spriteEntriesDict = new Dictionary<SpriteEntryKey, Sprite>();
            foreach (var entry in SpritesEntries)
            {
                if (!m_spriteEntriesDict.ContainsKey(entry.Key))
                    m_spriteEntriesDict.Add(entry.Key, entry.sprite);
            }
        }
    }

    public Sprite GetSprite(SpriteEntryKey key)
    {
        return m_spriteEntriesDict.TryGetValue(key, out var sprite) ? sprite : null;
    }

    public Sprite GetResourceIcon(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Popularity:
                return GetSprite(SpriteEntryKey.PopularityResource);
            case ResourceType.Stability:
                return GetSprite(SpriteEntryKey.StabilityResource);
            case ResourceType.Media:
                return GetSprite(SpriteEntryKey.MediaResource);
            case ResourceType.Economic:
                return GetSprite(SpriteEntryKey.EconomicResource);
            default: return null;
        }
    }

    public string GetCategoryDisplayName(CardCategory category)
    {
        if (m_CategoryDisplayNameMap == null)
        {
            m_CategoryDisplayNameMap = new();
            foreach (var pair in m_CategoryDisplayNames)
            {
                m_CategoryDisplayNameMap[pair.Category] = pair.DisplayName;
            }
        }

        return m_CategoryDisplayNameMap.TryGetValue(category, out var name) ? name : category.ToString();
    }

    public string GetCategoryUrgencyName(UrgencyLevel urgencyLevel)
    {
        if (m_UrgencyDisplayNameMap == null)
        {
            m_UrgencyDisplayNameMap = new();
            foreach (var pair in m_UrgencyDisplayNames)
            {
                m_UrgencyDisplayNameMap[pair.Category] = pair.DisplayName;
            }
        }

        return m_UrgencyDisplayNameMap.TryGetValue(urgencyLevel, out var name) ? name : urgencyLevel.ToString();
    }

    public string GetResourceTypeName(ResourceType resourceType)
    {
        if (m_ResourceDisplayNameMap == null)
        {
            m_ResourceDisplayNameMap = new();
            foreach (var pair in m_ResourceTypeDisplayNames)
            {
                m_ResourceDisplayNameMap[pair.ResourceType] = pair.DisplayName;
            }
        }

        return m_ResourceDisplayNameMap.TryGetValue(resourceType, out var name) ? name : resourceType.ToString();
    }

    /// <summary>
    /// Represents the sprite and its key
    /// </summary>
    [System.Serializable]
    public struct SpriteEntries
    {
        public SpriteEntryKey Key;
        public Sprite sprite;
    }

    public enum SpriteEntryKey
    {
        PopularityResource,
        StabilityResource,
        MediaResource,
        EconomicResource,
        StakeHolder,
    }
}

[System.Serializable]
public struct ResourceTypeDisplayName
{
    public ResourceType ResourceType;
    public string DisplayName;
}