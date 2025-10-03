#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class SubstateScriptCreator
{
    private const string Template =
@"using UnityEngine;

public class #SCRIPTNAME# : SubStateController
{
    protected override void OnEnter()
    {
        base.OnEnter();
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnSwitchDayState()
    {
        base.OnSwitchDayState();
    }
}
";

    [MenuItem("Assets/Create/Game/Substates/New Substate Script", false, 80)]
    public static void CreateSubstateScript()
    {
        string path = GetSelectedPathOrFallback();

        // Prompt user for script name
        string scriptName = EditorUtility.SaveFilePanelInProject(
            "Create Substate Script",
            "NewSubstate",
            "cs",
            "Enter a name for your new substate script.",
            path
        );

        if (string.IsNullOrEmpty(scriptName))
            return;

        string className = Path.GetFileNameWithoutExtension(scriptName);
        string content = Template.Replace("#SCRIPTNAME#", className);

        File.WriteAllText(scriptName, content);
        AssetDatabase.Refresh();

        Debug.Log($"Created substate script: {className}");
    }

    private static string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }
            break;
        }
        return path;
    }
}
#endif

