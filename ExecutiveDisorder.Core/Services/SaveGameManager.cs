using System.Text.Json;
using ExecutiveDisorder.Core.Models;

namespace ExecutiveDisorder.Core.Services;

public static class SaveGameManager
{
    private static readonly string SaveDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ExecutiveDisorder",
        "Saves"
    );

    static SaveGameManager()
    {
        Directory.CreateDirectory(SaveDirectory);
    }

    public static void SaveGame(SaveGame saveGame, string filename = "autosave.json")
    {
        var filePath = Path.Combine(SaveDirectory, filename);
        var json = JsonSerializer.Serialize(saveGame, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        File.WriteAllText(filePath, json);
    }

    public static SaveGame? LoadGame(string filename = "autosave.json")
    {
        var filePath = Path.Combine(SaveDirectory, filename);
        if (!File.Exists(filePath))
        {
            return null;
        }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<SaveGame>(json);
    }

    public static List<string> GetSaveFiles()
    {
        return Directory.GetFiles(SaveDirectory, "*.json")
            .Select(Path.GetFileName)
            .Where(f => f != null)
            .Select(f => f!)
            .ToList();
    }

    public static void DeleteSave(string filename)
    {
        var filePath = Path.Combine(SaveDirectory, filename);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public static string GetSavePath() => SaveDirectory;
}
