using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ending", menuName = "Game/Ending")]
public class EndingSO : ScriptableObject
{
    public string Title;
    [TextArea]
    public string Description;
    public List<EndingResourceRequirement> ResourceRequirements;
}

[Serializable]
public class EndingResourceRequirement
{
    public ResourceType ResourceType;
    public ComparisonType Comparison;
    public float Value;
}

public enum ComparisonType
{
    LessThan,
    Equal,
    NotEqual,
    GreaterThan,
}
