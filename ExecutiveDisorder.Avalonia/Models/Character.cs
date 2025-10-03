using System.Collections.Generic;
namespace ExecutiveDisorder.Avalonia.Models;

public class Character
{
    public string CharacterName { get; set; } = string.Empty;
    public string GovernTitle { get; set; } = string.Empty;
    public string PartyAffiliation { get; set; } = string.Empty;
    public string CampaignSlogan { get; set; } = string.Empty;
    public int InitialPopularity { get; set; }
    public int InitialStability { get; set; }
    public int InitialMedia { get; set; }
    public int InitialEconomic { get; set; }
}

public class CharactersData
{
    public List<Character> Characters { get; set; } = new();
}
