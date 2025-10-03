using UnityEngine;

[System.Serializable]
public class GameResources
{
    public float popularity = 50f;
    public float stability = 50f;
    public float media = 50f;
    public float economic = 50f;

    public const float MIN_VALUE = 0f;
    public const float MAX_VALUE = 100f;

    public void ModifyResource(string resourceName, float amount)
    {
        switch (resourceName.ToLower())
        {
            case "popularity":
                popularity = Mathf.Clamp(popularity + amount, MIN_VALUE, MAX_VALUE);
                break;
            case "stability":
                stability = Mathf.Clamp(stability + amount, MIN_VALUE, MAX_VALUE);
                break;
            case "media":
                media = Mathf.Clamp(media + amount, MIN_VALUE, MAX_VALUE);
                break;
            case "economic":
                economic = Mathf.Clamp(economic + amount, MIN_VALUE, MAX_VALUE);
                break;
        }
    }

    public bool IsGameOver()
    {
        return popularity <= MIN_VALUE || stability <= MIN_VALUE || 
               media <= MIN_VALUE || economic <= MIN_VALUE;
    }

    public string GetEndingType()
    {
        if (popularity <= 10) return "Overthrown";
        if (stability <= 10) return "Collapse";
        if (media <= 10) return "Censored";
        if (economic <= 10) return "Bankrupt";
        
        float avg = (popularity + stability + media + economic) / 4f;
        if (avg >= 80) return "GoldenAge";
        if (avg >= 70) return "Successful";
        if (avg >= 60) return "Balanced";
        if (avg >= 50) return "Struggling";
        if (avg >= 40) return "Crisis";
        return "Disaster";
    }
}
