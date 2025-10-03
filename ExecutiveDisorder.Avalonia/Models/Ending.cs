using System.Collections.Generic;
namespace ExecutiveDisorder.Avalonia.Models;

public class Ending
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<EndingRequirement> ResourceRequirements { get; set; } = new();
}

public class EndingRequirement
{
    public string ResourceType { get; set; } = string.Empty;
    public string Comparison { get; set; } = string.Empty;
    public int Value { get; set; }
}

public class EndingsData
{
    public List<Ending> Endings { get; set; } = new();
}
