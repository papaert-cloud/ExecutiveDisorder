using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character Effect", menuName = "Game/CharacterEffect")]
public class CharacterEffect : ScriptableObject
{
    public string Description;
    public List<ResourceAffected> ResourcesAffected = new List<ResourceAffected>();
}

[Serializable]
public class ResourceAffected
{
    public ResourceType ResourceType;
    public int Amount;
    public float Percentage;
}