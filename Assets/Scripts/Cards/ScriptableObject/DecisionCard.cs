using UnityEngine;
using System.Collections.Generic;
using System; // Required for DateTime

/// <summary>
/// Represents the category of the decision card.
/// </summary>
[System.Serializable]
public enum CardCategory
{
    Policy,
    Crisis,
    Character,
    Opportunity,
    SocialMedia,
    Legacy
}

/// <summary>
/// Represents the urgency level of the decision.
/// </summary>
[System.Serializable]
public enum UrgencyLevel
{
    Low,        // Can be ignored indefinitely (or for a long time)
    Medium,     // Standard decision timeframe
    High,       // Needs attention soon
    Immediate   // Requires immediate response (e.g., crisis)
}


/// <summary>
/// Represents the display name for the category
/// </summary>
[System.Serializable]
public struct CardCategoryDisplayName
{
    public CardCategory Category;
    public string DisplayName;
}

/// <summary>
/// Represents the display name for the urgency
/// </summary>
[System.Serializable]
public struct CardCUrgencyDisplayName
{
    public UrgencyLevel Category;
    public string DisplayName;
}

/// <summary>
/// Represents the stakeholder
/// </summary>
[System.Serializable]
public enum StakeHolders
{
    Tech,
    SomethingElse
}

/// <summary>
/// Represents the resource cost associated with a decision or option.
/// </summary>
[System.Serializable]
public struct ResourceCost
{
    [Tooltip("Type of resource (e.g., PoliticalCapital, Budget, StaffTime). Use string or enum.")]
    public ResourceType Type; // Consider using an enum for specific resource types

    [Tooltip("Amount of the resource required or gained (negative for cost).")]
    public float Amount;

    [Range(-100, 100)] // This attribute creates a slider in the Inspector
    [Tooltip("Percentage change of the resource required or gained (-100% to +100%). A negative value means a cost, positive means a gain.")]
    public int Percent; // New field with range slider
}

/// <summary>
/// Represents a single choice available to the player for a decision.
/// </summary>
[System.Serializable] // Makes this visible in the Unity Inspector
public class DecisionOption
{
    [Tooltip("The text displayed to the player for this option.")]
    public string OptionText;

    [Tooltip("Description of the immediate or likely outcome of choosing this option.")]
    public string OutcomeDescription;

    [Tooltip("Resources required or gained if this option is chosen.")]
    public List<ResourceCost> ResourceRequirements;

    [Tooltip("Notes on how specific stakeholders might react *to this specific option*.")]
    public List<string> StakeholderReactions; // E.g., "Labor Union supports", "Big Corp opposes"

    [Tooltip("which specific stakeholders might get affected by *this specific option*.")]
    public List<StakeHolders> StakeholdersAffected; // E.g., "Labor Union supports", "Big Corp opposes"

    public int ConsequenceCardID = -1;

    public bool IsDelayOption;

    public ReactionType ReactionType;

    public string NewsHeadline;

    public Dictionary<ResourceType, float> GetResourcesAffected()
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
                costByType[resourceCost.Type] += resourceCost.Amount;
        }

        Character character = CharacterManager.Instance.CurrentCharacter;
        foreach (ResourceAffected resourceAffected in character.CharacterEffect.ResourcesAffected)
        {
            if (costByType.ContainsKey(resourceAffected.ResourceType))
                costByType[resourceAffected.ResourceType] += resourceAffected.Amount;
        }

        return costByType;
    }

    // You might add more fields here later, e.g.:
    // public FactionReputationChanges reputationImpact;
    // public string ConsequenceID; // To trigger specific follow-up events
}

/// <summary>
/// Represents elements that can be combined for custom responses.
/// </summary>
[System.Serializable]
public enum ResponseElement
{
    // Add specific elements based on your game's needs
    None,
    EmptyPromises,
    PatrioticRhetoric,
    FlagWaving,
    BlamePredecessor,
    VagueCommitment,
    TechnicalJargon,
    DemandFurtherStudy,
    AcknowledgeConcern // etc.
}

/// <summary>
/// Represents a decision card presented to the player, incorporating anatomy details.
/// </summary>
[CreateAssetMenu(fileName = "NewDecisionCard", menuName = "Game/Decision Card")] // Allows creating instances from Unity Editor
public class DecisionCard : ScriptableObject
{
    [Header("Card Anatomy")]
    [Tooltip("A unique identifier for this card.")]
    public int CardID;

    [Tooltip("The main title or question presented on the card.")]
    public string Title;

    [Tooltip("The category this card falls into (e.g., Policy, Crisis).")]
    public CardCategory Category;

    [TextArea(3, 10)] 
    [Tooltip("The detailed description or situation presented to the player.")]
    public string Description;

    [Tooltip("The Card image")]
    public Sprite CardSprite;

    [Tooltip("General notes about stakeholders involved or interested in this issue overall.")]
    public List<string> StakeholderInformation; // E.g., "Affects Tech Industry", "Environmental groups watching"

    [Tooltip("Indicates how quickly a decision is needed. Affects UI and potentially gameplay urgency.")]
    public UrgencyLevel Urgency;

    [Tooltip("If this card is a Consequence of another")]
    public bool IsConsequence;

    [Header("Decision Options")]
    [Tooltip("List of pre-defined choices for the player. Each option can have its own costs and stakeholder reactions.")]
    public List<DecisionOption> MultipleChoiceOptions;

    [Header("Media Reactions")]
    public List<MediaReaction> MediaReactions;

    public List<MediaReaction> GetMediaReactionsByType(ReactionType type)
    {
        return MediaReactions?.FindAll(reaction => reaction.ReactionType == type) ?? new List<MediaReaction>();
    }

    [Header("Custom Response")]
    [Tooltip("Can the player craft a custom response for this card?")]
    public bool AllowCustomResponseCrafting;

    [Tooltip("If custom responses are allowed, which elements can be used?")]
    public List<ResponseElement> AvailableResponseElements;
    // Note: The actual crafting UI and logic would be separate.
    // The player's crafted response (e.g., List<ResponseElement>) would likely be stored temporarily elsewhere.

    [Header("Timing")]
    [Tooltip("Does this decision have a time limit in seconds? (Set to 0 for no specific countdown, rely on Urgency).")]
    public float TimeLimitSeconds = 0f; // Use float for easy countdown in Unity

    [Tooltip("Does the outcome change based on how quickly the decision is made? (Relevant if TimeLimitSeconds > 0)")]
    public bool TimingAffectsOutcome;
    // Note: Logic for varying outcomes based on time would likely be handled by the game manager
    // checking how much time was remaining when the decision was made.

    [Header("Advanced Mechanics")]
    [Tooltip("Can this decision be delegated?")]
    public bool CanDelegate;

    [Tooltip("Identifier for the advisor/department type this can be delegated to (e.g., 'EnvironmentSecretary', 'Treasury').")]
    public string DelegateTargetID; // Use an ID or enum based on your advisor system

    [Tooltip("Cost (e.g., resources, time) to gather information.")]
    public List<ResourceCost> InfoGatheringCost; // Changed to list for flexibility

    [TextArea(2, 5)]
    [Tooltip("The extra information revealed if the player chooses to gather it.")]
    public string AdditionalInfo;

    [Tooltip("Can the player consult advisors or stakeholders?")]
    public bool CanConsult;

    [Tooltip("List of stakeholder/advisor IDs the player can consult (e.g., 'OilLobby', 'ChiefOfStaff').")]
    public List<string> ConsultationTargets; // Use IDs or enums

    [Tooltip("Can the player defer this decision?")]
    public bool CanDefer;

    [Tooltip("Identifier for a potential follow-up or escalated card if this is deferred too long.")]
    public string DeferredConsequenceCardID; // Link to another DecisionCard ScriptableObject ID

    [Tooltip("Card for a crisis card if this is delayed too long.")]
    public int CrisisCardID;

    // --- Runtime Variables (Not serialized directly on the asset, but managed during gameplay) ---
    // [HideInInspector] public DateTime? DeadlineTimestamp; // If using real-time deadlines
    // [HideInInspector] public int DeferralCount = 0; // Track how many times it's been deferred

    // --- Methods (Example - Implementation would be in your game logic controllers) ---

    // public void ChooseOption(int optionIndex) { ... }
    // public void CraftResponse(List<ResponseElement> chosenElements) { ... }
    // public void DelegateDecision() { ... }
    // public void GatherInformation() { ... }
    // public void Consult(string targetID) { ... }
    // public void Defer() { ... }
}
