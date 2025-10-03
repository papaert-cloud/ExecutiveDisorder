using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;

public class BuildScript
{
    [MenuItem("Build/Build WebGL")]
    public static void BuildWebGL()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { 
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/Crisis.unity",
            "Assets/Scenes/Gameplay.unity"
        };
        buildPlayerOptions.locationPathName = "Builds/WebGL";
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.None;

        Debug.Log("Starting WebGL build...");
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded: {summary.totalSize} bytes");
            Debug.Log($"Build time: {summary.totalTime}");
            EditorUtility.DisplayDialog("Build Complete", 
                $"WebGL build completed successfully!\n\n" +
                $"Size: {FormatBytes(summary.totalSize)}\n" +
                $"Time: {summary.totalTime}\n" +
                $"Location: Builds/WebGL", 
                "OK");
        }
        else if (summary.result == BuildResult.Failed)
        {
            Debug.LogError("Build failed");
            EditorUtility.DisplayDialog("Build Failed", 
                "WebGL build failed! Check console for errors.", 
                "OK");
        }
    }

    [MenuItem("Build/Build All Platforms")]
    public static void BuildAllPlatforms()
    {
        BuildWebGL();
        // Add other platforms here if needed
    }

    private static string FormatBytes(ulong bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }

    // Command line build method
    public static void BuildFromCommandLine()
    {
        BuildWebGL();
    }
}
