#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class EndingSOImporter : EditorWindow
{
    [MenuItem("Tools/Import Endings from JSON")]
    public static void ImportEndings()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Endings JSON File", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath)) return;

        string json = File.ReadAllText(jsonPath);

        EndingJsonWrapperRaw rawWrapper = JsonUtility.FromJson<EndingJsonWrapperRaw>(json);
        if (rawWrapper == null || rawWrapper.endings == null)
        {
            Debug.LogError("Failed to parse JSON");
            return;
        }

        string folderPath = "Assets/Data/Endings";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Data"))
                AssetDatabase.CreateFolder("Assets", "Data");

            AssetDatabase.CreateFolder("Assets/Data", "Endings");
        }

        foreach (var rawEnding in rawWrapper.endings)
        {
            EndingSO endingSO = ScriptableObject.CreateInstance<EndingSO>();
            endingSO.Title = rawEnding.Title;
            endingSO.Description = rawEnding.Description;
            endingSO.ResourceRequirements = new List<EndingResourceRequirement>();

            if (rawEnding.ResourceRequirements != null)
            {
                foreach (var rawReq in rawEnding.ResourceRequirements)
                {
                    if (Enum.TryParse(rawReq.ResourceType, true, out ResourceType parsedType) &&
                        Enum.TryParse(rawReq.Comparison, true, out ComparisonType parsedComp))
                    {
                        endingSO.ResourceRequirements.Add(new EndingResourceRequirement
                        {
                            ResourceType = parsedType,
                            Comparison = parsedComp,
                            Value = rawReq.Value
                        });
                    }
                    else
                    {
                        Debug.LogWarning($"Invalid enum values in ending '{rawEnding.Title}'");
                    }
                }
            }

            string assetPath = $"{folderPath}/Ending_{endingSO.Title.Replace(" ", "_")}.asset";
            AssetDatabase.CreateAsset(endingSO, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Endings imported!");
    }
}

// -------- RAW JSON CLASSES --------

[Serializable]
public class EndingJsonWrapperRaw
{
    public EndingJsonDataRaw[] endings;
}

[Serializable]
public class EndingJsonDataRaw
{
    public string Title;
    public string Description;
    public EndingRequirementRaw[] ResourceRequirements;
}

[Serializable]
public class EndingRequirementRaw
{
    public string ResourceType;
    public string Comparison;
    public float Value;
}
#endif
