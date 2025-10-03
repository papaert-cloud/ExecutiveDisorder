using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Executive Disorder/Character")]
public class PoliticalCharacter : ScriptableObject
{
    public string characterName;
    public string title;
    public Sprite portrait;
    [TextArea(3, 10)]
    public string biography;
    public string ideology;
    
    // Character traits affect decision outcomes
    public float popularityInfluence = 1f;
    public float stabilityInfluence = 1f;
    public float mediaInfluence = 1f;
    public float economicInfluence = 1f;
}
