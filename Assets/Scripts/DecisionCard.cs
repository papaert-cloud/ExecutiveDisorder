using UnityEngine;

[System.Serializable]
public class DecisionConsequence
{
    public string resourceName;
    public float amount;
    public string visualEffect;
    public string audioClip;
    public string newsHeadline;
    [TextArea(2, 5)]
    public string socialMediaReaction;
}

[CreateAssetMenu(fileName = "New Decision", menuName = "Executive Disorder/Decision Card")]
public class DecisionCard : ScriptableObject
{
    public int cardId;
    public string cardTitle;
    [TextArea(3, 10)]
    public string description;
    public Sprite cardImage;
    
    public string option1Text;
    public string option2Text;
    
    public DecisionConsequence[] option1Consequences;
    public DecisionConsequence[] option2Consequences;
    
    // Cascade effects - these cards trigger after this decision
    public DecisionCard[] cascadeCards;
    public float cascadeProbability = 0.3f;
}
