using System.Text.Json;
using ExecutiveDisorder.Core.Models;

namespace ExecutiveDisorder.Core.Services;

public class GameDataLoader
{
    private static string ResolveDataPath(string fileName)
    {
        var baseDir = AppContext.BaseDirectory;
        
        // Try: bin/Debug|Release/net9.0/Data/filename.json (published/deployed)
        var dataPath = Path.Combine(baseDir, "Data", fileName);
        if (File.Exists(dataPath)) return dataPath;
        
        // Try: ../Assets/filename.json (dev run from project root)
        var assetsPath = Path.Combine(baseDir, "..", "Assets", fileName);
        if (File.Exists(assetsPath)) return Path.GetFullPath(assetsPath);
        
        // Try: ../../Assets/filename.json (dev run from bin folder)
        var assetsPath2 = Path.Combine(baseDir, "..", "..", "Assets", fileName);
        if (File.Exists(assetsPath2)) return Path.GetFullPath(assetsPath2);
        
        // Try: ../../../Assets/filename.json (dev run from bin/Debug/net9.0)
        var assetsPath3 = Path.Combine(baseDir, "..", "..", "..", "Assets", fileName);
        if (File.Exists(assetsPath3)) return Path.GetFullPath(assetsPath3);
        
        throw new FileNotFoundException($"Could not find {fileName} in any expected location. Searched: {dataPath}, {assetsPath}, {assetsPath2}, {assetsPath3}");
    }

    public static CharacterList LoadCharacters()
    {
        var filePath = ResolveDataPath("charactersjson.json");
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<CharacterList>(json) ?? new CharacterList();
    }

    public static DecisionCardList LoadCards()
    {
        var filePath = ResolveDataPath("cardsjson.json");
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<DecisionCardList>(json) ?? new DecisionCardList();
    }

    public static EndingList LoadEndings()
    {
        var filePath = ResolveDataPath("endingjson.json");
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<EndingList>(json) ?? new EndingList();
    }
}
