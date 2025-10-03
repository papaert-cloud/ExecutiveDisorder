#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class CharacterSOImporter : EditorWindow
{
    [MenuItem("Tools/Import Characters from JSON")]
    public static void ImportCharacters()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Characters JSON File", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath)) return;

        string json = File.ReadAllText(jsonPath);

        CharacterJsonWrapperRaw rawWrapper = JsonUtility.FromJson<CharacterJsonWrapperRaw>(json);
        if (rawWrapper == null || rawWrapper.characters == null)
        {
            Debug.LogError("Failed to parse JSON");
            return;
        }

        string folderPath = "Assets/Data/Characters";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Data"))
                AssetDatabase.CreateFolder("Assets", "Data");

            AssetDatabase.CreateFolder("Assets/Data", "Characters");
        }

        foreach (var rawChar in rawWrapper.characters)
        {
            Character characterSO = ScriptableObject.CreateInstance<Character>();
            characterSO.CharacterName = rawChar.CharacterName;
            characterSO.GovernTitle = rawChar.GovernTitle;
            characterSO.PartyAffiliation = rawChar.PartyAffiliation;
            characterSO.CampaignSlogan = rawChar.CampaignSlogan;
            characterSO.InitialPopularity = rawChar.InitialPopularity;
            characterSO.InitialStability = rawChar.InitialStability;
            characterSO.InitialMedia = rawChar.InitialMedia;
            characterSO.InitialEconomic = rawChar.InitialEconomic;

            // You can set sprites manually in the editor later
            characterSO.Portrait = null;
            characterSO.FullBody = null;
            characterSO.ProfilePicture = null;

            string assetPath = $"{folderPath}/Character_{characterSO.CharacterName.Replace(" ", "_")}.asset";
            AssetDatabase.CreateAsset(characterSO, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Characters imported!");
    }
}

// -------- RAW JSON CLASSES --------

[Serializable]
public class CharacterJsonWrapperRaw
{
    public CharacterJsonDataRaw[] characters;
}

[Serializable]
public class CharacterJsonDataRaw
{
    public string CharacterName;
    public string GovernTitle;
    public string PartyAffiliation;
    public string CampaignSlogan;

    public float InitialPopularity;
    public float InitialStability;
    public float InitialMedia;
    public float InitialEconomic;
}
#endif
