#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using static UnityEngine.Timeline.TimelineAsset;

public class DecisionCardImporter : EditorWindow
{
    [MenuItem("Tools/Import Decision Cards from JSON (Simplified)")]
    public static void ImportCards()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select JSON File", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath)) return;

        string json = File.ReadAllText(jsonPath);

        DecisionCardJsonWrapperRaw rawWrapper = JsonUtility.FromJson<DecisionCardJsonWrapperRaw>(json);
        if (rawWrapper == null || rawWrapper.cards == null)
        {
            Debug.LogError("Failed to parse JSON");
            return;
        }

        string folderPath = "Assets/Data/DecisionCards";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Data"))
                AssetDatabase.CreateFolder("Assets", "Data");

            AssetDatabase.CreateFolder("Assets/Data", "DecisionCards");
        }

        string mediaFolderPath = "Assets/Data/MediaReactions";
        if (!AssetDatabase.IsValidFolder(mediaFolderPath))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Data"))
                AssetDatabase.CreateFolder("Assets", "Data");

            AssetDatabase.CreateFolder("Assets/Data", "MediaReactions");
        }

        foreach (var rawCard in rawWrapper.cards)
        {
            if (!Enum.TryParse(rawCard.Category, true, out CardCategory parsedCategory))
                parsedCategory = CardCategory.Policy;

            if (!Enum.TryParse(rawCard.Urgency, true, out UrgencyLevel parsedUrgency))
                parsedUrgency = UrgencyLevel.Medium;

            DecisionCard card = ScriptableObject.CreateInstance<DecisionCard>();
            card.CardID = rawCard.CardID;
            card.Title = rawCard.Title;
            card.Description = rawCard.Description;
            card.Category = parsedCategory;
            card.Urgency = parsedUrgency;
            card.CrisisCardID = rawCard.CrisisCardID;
            card.IsConsequence = rawCard.IsConsequence;
            card.MultipleChoiceOptions = new List<DecisionOption>();
            card.MediaReactions = new List<MediaReaction>();

            foreach (var jsonOption in rawCard.MultipleChoiceOptions)
            {
                bool parsedReaction = Enum.TryParse(jsonOption.ReactionType, true, out ReactionType reactionType);
                var option = new DecisionOption
                {
                    OptionText = jsonOption.OptionText,
                    OutcomeDescription = jsonOption.OutcomeDescription,
                    ResourceRequirements = new List<ResourceCost>(),
                    StakeholderReactions = new List<string>(jsonOption.StakeholderReactions ?? new string[0]),
                    StakeholdersAffected = new List<StakeHolders>(),
                    ConsequenceCardID = jsonOption.ConsequenceCardID,
                    IsDelayOption = jsonOption.IsDelayOption,
                    ReactionType = parsedReaction ? reactionType : ReactionType.Neutral,
            };

                if (jsonOption.ResourceRequirements != null)
                {
                    foreach (var rawCost in jsonOption.ResourceRequirements)
                    {
                        if (Enum.TryParse(rawCost.Type, true, out ResourceType parsedResourceType))
                        {
                            option.ResourceRequirements.Add(new ResourceCost
                            {
                                Type = parsedResourceType,
                                Amount = rawCost.Amount,
                                Percent = rawCost.Percent
                            });
                        }
                        else
                        {
                            Debug.LogWarning($"Unknown resource type '{rawCost.Type}' in Card {rawCard.CardID}");
                        }
                    }
                }

                if (jsonOption.StakeholdersAffected != null)
                {
                    foreach (var s in jsonOption.StakeholdersAffected)
                    {
                        if (Enum.TryParse(s, out StakeHolders parsedStakeholder))
                            option.StakeholdersAffected.Add(parsedStakeholder);
                        else
                            Debug.LogWarning($"Unknown stakeholder '{s}' in Card {rawCard.CardID}");
                    }
                }

                card.MultipleChoiceOptions.Add(option);
            }

            // Process Media Reactions
            if (rawCard.MediaReactions != null)
            {
                for (int i = 0; i < rawCard.MediaReactions.Length; i++)
                {
                    var rawReaction = rawCard.MediaReactions[i];
                    bool parsedReaction = Enum.TryParse(rawReaction.ReactionType, true, out ReactionType reactionType);

                    MediaReaction reactionAsset = ScriptableObject.CreateInstance<MediaReaction>();
                    reactionAsset.ReactionType = parsedReaction ? reactionType : ReactionType.Neutral;
                    reactionAsset.Text = rawReaction.Text;
                    reactionAsset.Topic = rawReaction.Topic;
                    reactionAsset.SourceName = rawReaction.SourceName;
                    reactionAsset.ResourceRequirements = new List<ResourceCost>();

                    if (rawReaction.ResourceRequirements != null)
                    {
                        foreach (var rawCost in rawReaction.ResourceRequirements)
                        {
                            if (Enum.TryParse(rawCost.Type, true, out ResourceType parsedResourceType))
                            {
                                reactionAsset.ResourceRequirements.Add(new ResourceCost
                                {
                                    Type = parsedResourceType,
                                    Amount = rawCost.Amount,
                                    Percent = rawCost.Percent
                                });
                            }
                            else
                            {
                                Debug.LogWarning($"Unknown resource type '{rawCost.Type}' in MediaReaction of Card {rawCard.CardID}");
                            }
                        }
                    }

                    string reactionAssetPath = $"{mediaFolderPath}/MediaReaction_{card.CardID}_{i}.asset";
                    AssetDatabase.CreateAsset(reactionAsset, reactionAssetPath);
                    card.MediaReactions.Add(reactionAsset);
                }
            }


            string assetPath = $"{folderPath}/DecisionCard_{card.CardID}.asset";
            AssetDatabase.CreateAsset(card, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Cards and media reactions imported!");
    }
}

// -------- RAW JSON CLASSES --------

[System.Serializable]
public class DecisionCardJsonWrapperRaw
{
    public DecisionCardJsonDataRaw[] cards;
}

[System.Serializable]
public class DecisionCardJsonDataRaw
{
    public int CardID;
    public string Title;
    public string Description;
    public string Category;
    public string Urgency;
    public int CrisisCardID;
    public bool IsConsequence;
    public DecisionOptionJsonDataRaw[] MultipleChoiceOptions;
    public MediaReactionJsonRaw[] MediaReactions;
}

[System.Serializable]
public class DecisionOptionJsonDataRaw
{
    public string OptionText;
    public string OutcomeDescription;
    public ResourceCostRaw[] ResourceRequirements;
    public string[] StakeholderReactions;
    public string[] StakeholdersAffected;
    public int ConsequenceCardID;
    public bool IsDelayOption;
    public string ReactionType;
}

[System.Serializable]
public struct ResourceCostRaw
{
    public string Type;
    public float Amount;
    public int Percent;
}

[System.Serializable]
public class MediaReactionJsonRaw
{
    public string ReactionType;
    public string Text;
    public string Topic;
    public string SourceName;
    public ResourceCostRaw[] ResourceRequirements;
}
#endif
