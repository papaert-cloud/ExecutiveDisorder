using UnityEngine;

[CreateAssetMenu(fileName = "NewPoliticalCharacter", menuName = "Game/PoliticalCharacter")]
public class Character : ScriptableObject
{
    public string CharacterName;
    public string GovernTitle;
    public string PartyAffiliation;
    public Sprite Portrait;
    public Sprite FullBody;
    public Sprite ProfilePicture;

    [TextArea]
    public string CampaignSlogan;

    public CharacterEffect CharacterEffect;

    [Header("Initial Resource Values")]
    [Tooltip("Starting value for Popularity")]
    public float InitialPopularity = 50f; // Example starting value
    [Tooltip("Starting value for Stability")]
    public float InitialStability = 50f; // Example starting value
    [Tooltip("Starting value for Media Control")]
    public float InitialMedia = 50f; // Example starting value
    [Tooltip("Starting value for Economic Strength")]
    public float InitialEconomic = 50f; // Example starting value
}
