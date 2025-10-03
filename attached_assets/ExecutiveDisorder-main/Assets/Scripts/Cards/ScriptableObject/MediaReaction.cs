using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum ReactionType { Positive, Negative, Neutral }

[CreateAssetMenu(fileName = "NewMediaReaction", menuName = "PoliticalGame/Media Reaction")]
public class MediaReaction : ScriptableObject
{
    [Header("Media Metadata")]
    public ReactionType ReactionType;

    [Header("Displayed Content")]
    [TextArea(2, 5)]
    public string Text;

    [Header("Associated Tags (Optional)")]
    public string Topic;     
    public string SourceName;

    public List<ResourceCost> ResourceRequirements;

    public Dictionary<ResourceType, float> GetResourcesAffected(bool negative=false)
    {
        Dictionary<ResourceType, float> costByType = new Dictionary<ResourceType, float>
        {
            { ResourceType.Popularity, 0f },
            { ResourceType.Stability, 0f },
            { ResourceType.Media, 0f },
            { ResourceType.Economic, 0f }
        };

        foreach (var resourceCost in ResourceRequirements)
        {
            if (costByType.ContainsKey(resourceCost.Type))
            {
                if(!negative) costByType[resourceCost.Type] += resourceCost.Amount;
                else costByType[resourceCost.Type] -= resourceCost.Amount;
            }
                
        }

        Character character = CharacterManager.Instance.CurrentCharacter;
        foreach (ResourceAffected resourceAffected in character.CharacterEffect.ResourcesAffected)
        {
            if (costByType.ContainsKey(resourceAffected.ResourceType))
                costByType[resourceAffected.ResourceType] += resourceAffected.Amount;
        }

        return costByType;
    }
}
