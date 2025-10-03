using System.Collections.Generic;
namespace ExecutiveDisorder.Avalonia.Models;

public class DecisionCard
{
    public int CardID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Urgency { get; set; } = string.Empty;
    public bool IsConsequence { get; set; }
    public List<ChoiceOption> MultipleChoiceOptions { get; set; } = new();
    public int CrisisCardID { get; set; }
}

public class ChoiceOption
{
    public string OptionText { get; set; } = string.Empty;
    public string OutcomeDescription { get; set; } = string.Empty;
    public List<ResourceRequirement> ResourceRequirements { get; set; } = new();
    public List<string> StakeholdersAffected { get; set; } = new();
    public int ConsequenceCardID { get; set; }
    public bool IsDelayOption { get; set; }
}

public class ResourceRequirement
{
    public string Type { get; set; } = string.Empty;
    public double Amount { get; set; }
}

public class CardsData
{
    public List<DecisionCard> Cards { get; set; } = new();
}
