using TMPro;
using UnityEngine;

public class EndingResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ResourceText;

    public void SetEndingResource(ResourceType ResourceType, ComparisonType Comparison, float Value)
    {
        string comparison  = GetComparisonString(Comparison);

        m_ResourceText.text = $"{ResourceType} {comparison} {Value}";
    }

    private string GetComparisonString(ComparisonType Comparison)
    {
        string OutString = "";

        switch (Comparison)
        {
            case ComparisonType.Equal:
                OutString = "=";
                break;
            case ComparisonType.NotEqual:
                OutString = "!=";
                break;
            case ComparisonType.GreaterThan:
                OutString = ">";
                break;
            case ComparisonType.LessThan:
                OutString = "<";
                break;
        }

        return OutString;
    }
}
