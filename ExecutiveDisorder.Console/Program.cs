using ExecutiveDisorder.Core.Models;
using ExecutiveDisorder.Core.Services;

namespace ExecutiveDisorder.Console;

class Program
{
    private static GameResources? resources;
    private static List<string> decisionLog = new();
    private static List<string> mediaHeadlines = new();
    private const int DecisionTimeLimit = 30; // seconds

    static void Main(string[] args)
    {
        try
        {
            // Harden console: Check minimum window size
            if (!EnsureConsoleDimensions())
            {
                System.Console.WriteLine("ERROR: Console window too small. Minimum 80x25 required.");
                System.Console.WriteLine("Press any key to exit...");
                System.Console.ReadKey();
                return;
            }

            System.Console.Title = "Executive Disorder - Political Decision Game";
            System.Console.CursorVisible = false;

            // Load game data
            var characters = GameDataLoader.LoadCharacters();
            var cards = GameDataLoader.LoadCards();
            var endings = GameDataLoader.LoadEndings();

            if (characters.Characters.Count == 0 || cards.Cards.Count == 0)
            {
                System.Console.WriteLine("ERROR: Failed to load game data. Check JSON files exist.");
                System.Console.ReadKey();
                return;
            }

            // Character selection
            var selectedCharacter = SelectCharacter(characters.Characters);
            if (selectedCharacter == null) return;

            // Initialize resources
            resources = new GameResources(
                selectedCharacter.StartingPopularity,
                selectedCharacter.StartingStability,
                selectedCharacter.StartingMediaTrust,
                selectedCharacter.StartingEconomic
            );

            // Shuffle cards for variety
            var random = new Random();
            var shuffledCards = cards.Cards.OrderBy(_ => random.Next()).ToList();

            // Game loop
            int decisionsCount = 0;
            foreach (var card in shuffledCards)
            {
                System.Console.Clear();
                DrawGameHeader(selectedCharacter.Name, decisionsCount);
                DrawResources();
                DrawRecentHeadlines();

                if (!PresentDecision(card))
                {
                    // User quit
                    return;
                }

                decisionsCount++;

                // Check for game over
                if (resources.IsGameOver())
                {
                    ShowGameOver(endings.Endings, decisionsCount);
                    return;
                }

                // Check for ending after certain decisions
                if (decisionsCount >= 15)
                {
                    var ending = CheckForEnding(endings.Endings);
                    if (ending != null)
                    {
                        ShowEnding(ending, decisionsCount);
                        return;
                    }
                }

                System.Threading.Thread.Sleep(1500); // Brief pause
            }

            // Completed all cards
            ShowEnding(endings.Endings[0], decisionsCount);
        }
        catch (Exception ex)
        {
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"FATAL ERROR: {ex.Message}");
            System.Console.ResetColor();
            System.Console.WriteLine("\nPress any key to exit...");
            System.Console.ReadKey();
        }
    }

    static bool EnsureConsoleDimensions()
    {
        try
        {
            return System.Console.WindowWidth >= 80 && System.Console.WindowHeight >= 25;
        }
        catch
        {
            // Some environments don't support window size queries
            return true;
        }
    }

    static Character? SelectCharacter(List<Character> characters)
    {
        System.Console.Clear();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════╗");
        System.Console.WriteLine("║              EXECUTIVE DISORDER - SELECT YOUR CHARACTER                   ║");
        System.Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════╝");
        System.Console.ResetColor();
        System.Console.WriteLine();

        for (int i = 0; i < characters.Count; i++)
        {
            var character = characters[i];
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine($"[{i + 1}] {character.Name} - {character.ArchetypeName}");
            System.Console.ResetColor();
            System.Console.WriteLine($"    {character.Description}");
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"    Starting: Pop={character.StartingPopularity} Stab={character.StartingStability} Media={character.StartingMediaTrust} Econ={character.StartingEconomic}");
            System.Console.ResetColor();
            System.Console.WriteLine();
        }

        System.Console.WriteLine("Enter character number (or 'q' to quit): ");
        System.Console.CursorVisible = true;

        while (true)
        {
            var input = System.Console.ReadLine()?.Trim().ToLower();
            
            if (input == "q" || input == "quit")
            {
                return null;
            }

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= characters.Count)
            {
                System.Console.CursorVisible = false;
                return characters[choice - 1];
            }

            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"Invalid input. Enter 1-{characters.Count} or 'q': ");
            System.Console.ResetColor();
        }
    }

    static void DrawGameHeader(string characterName, int decisionsCount)
    {
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════╗");
        System.Console.WriteLine($"║  {characterName,-60} Decisions: {decisionsCount,2}  ║");
        System.Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════╝");
        System.Console.ResetColor();
    }

    static void DrawResources()
    {
        if (resources == null) return;

        System.Console.Write("Resources: ");
        DrawResourceBar("Pop", resources.Popularity, ConsoleColor.Green);
        System.Console.Write(" | ");
        DrawResourceBar("Stab", resources.Stability, ConsoleColor.Blue);
        System.Console.Write(" | ");
        DrawResourceBar("Media", resources.MediaTrust, ConsoleColor.Magenta);
        System.Console.Write(" | ");
        DrawResourceBar("Econ", resources.Economic, ConsoleColor.Yellow);
        System.Console.WriteLine();
        System.Console.WriteLine();
    }

    static void DrawResourceBar(string name, int value, ConsoleColor color)
    {
        System.Console.Write($"{name}: ");
        System.Console.ForegroundColor = value < 20 ? ConsoleColor.Red : value < 50 ? ConsoleColor.Yellow : color;
        System.Console.Write($"{value,3}");
        System.Console.ResetColor();
    }

    static void DrawRecentHeadlines()
    {
        if (mediaHeadlines.Count == 0) return;

        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.WriteLine("Recent Headlines:");
        foreach (var headline in mediaHeadlines.TakeLast(3))
        {
            System.Console.WriteLine($"  • {headline}");
        }
        System.Console.ResetColor();
        System.Console.WriteLine();
    }

    static bool PresentDecision(DecisionCard card)
    {
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine("SITUATION:");
        System.Console.WriteLine(WrapText(card.Situation, 76));
        System.Console.ResetColor();
        System.Console.WriteLine();

        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine($"You have {DecisionTimeLimit} seconds to decide:");
        System.Console.ResetColor();

        for (int i = 0; i < card.Choices.Count; i++)
        {
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine($"[{i + 1}] {card.Choices[i].ChoiceText}");
            System.Console.ResetColor();
        }
        System.Console.WriteLine();

        // Timed decision with countdown
        var choice = GetTimedChoice(card.Choices.Count);

        if (choice == -1)
        {
            return false; // User quit
        }

        if (choice == 0)
        {
            // Timeout - random choice
            choice = new Random().Next(1, card.Choices.Count + 1);
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"\nTIME'S UP! Auto-selected option {choice}");
            System.Console.ResetColor();
            System.Threading.Thread.Sleep(1500);
        }

        ApplyChoice(card, choice - 1);
        return true;
    }

    static int GetTimedChoice(int maxChoice)
    {
        var startTime = DateTime.Now;
        var endTime = startTime.AddSeconds(DecisionTimeLimit);
        
        System.Console.Write($"Choice (1-{maxChoice} or 'q'): ");
        System.Console.CursorVisible = true;

        string input = "";
        while (DateTime.Now < endTime)
        {
            if (System.Console.KeyAvailable)
            {
                var key = System.Console.ReadKey(intercept: true);
                
                if (key.Key == ConsoleKey.Enter && input.Length > 0)
                {
                    System.Console.WriteLine();
                    System.Console.CursorVisible = false;

                    if (input.ToLower() == "q" || input.ToLower() == "quit")
                    {
                        return -1;
                    }

                    if (int.TryParse(input, out int choice) && choice >= 1 && choice <= maxChoice)
                    {
                        return choice;
                    }

                    // Invalid input
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.Write($"Invalid! Enter 1-{maxChoice}: ");
                    System.Console.ResetColor();
                    input = "";
                    continue;
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input[..^1];
                    System.Console.Write("\b \b");
                }
                else if (char.IsLetterOrDigit(key.KeyChar))
                {
                    input += key.KeyChar;
                    System.Console.Write(key.KeyChar);
                }
            }

            // Show countdown
            var remaining = (int)(endTime - DateTime.Now).TotalSeconds;
            if (remaining >= 0)
            {
                var cursorLeft = System.Console.CursorLeft;
                var cursorTop = System.Console.CursorTop;
                System.Console.SetCursorPosition(0, cursorTop + 1);
                System.Console.ForegroundColor = remaining <= 5 ? ConsoleColor.Red : ConsoleColor.DarkGray;
                System.Console.Write($"Time remaining: {remaining,2}s ");
                System.Console.ResetColor();
                System.Console.SetCursorPosition(cursorLeft, cursorTop);
            }

            System.Threading.Thread.Sleep(100);
        }

        System.Console.WriteLine();
        System.Console.CursorVisible = false;
        return 0; // Timeout
    }

    static void ApplyChoice(DecisionCard card, int choiceIndex)
    {
        var choice = card.Choices[choiceIndex];
        
        // Apply resource changes
        resources?.ApplyEffects(
            choice.PopularityEffect,
            choice.StabilityEffect,
            choice.MediaTrustEffect,
            choice.EconomicEffect
        );

        // Log decision
        decisionLog.Add($"Decision {decisionLog.Count + 1}: {choice.ChoiceText}");

        // Add media headline
        if (card.MediaReactions.Count > 0)
        {
            var reaction = card.MediaReactions[new Random().Next(card.MediaReactions.Count)];
            if (reaction.Reactions.Count > 0)
            {
                var headline = reaction.Reactions[new Random().Next(reaction.Reactions.Count)];
                mediaHeadlines.Add($"[{reaction.Outlet}] {headline}");
            }
        }

        // Show outcome
        if (choice.Outcomes.Count > 0)
        {
            System.Console.WriteLine();
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Outcome:");
            System.Console.WriteLine(choice.Outcomes[0]);
            System.Console.ResetColor();
            System.Threading.Thread.Sleep(2000);
        }
    }

    static Ending? CheckForEnding(List<Ending> endings)
    {
        if (resources == null) return null;

        foreach (var ending in endings)
        {
            if (MeetsRequirements(ending.ResourceRequirements))
            {
                return ending;
            }
        }
        return null;
    }

    static bool MeetsRequirements(ResourceRequirement req)
    {
        if (resources == null) return false;

        return CheckRequirement(req.Popularity, resources.Popularity) &&
               CheckRequirement(req.Stability, resources.Stability) &&
               CheckRequirement(req.MediaTrust, resources.MediaTrust) &&
               CheckRequirement(req.Economic, resources.Economic);
    }

    static bool CheckRequirement(string requirement, int value)
    {
        if (string.IsNullOrEmpty(requirement)) return true;

        if (requirement.StartsWith(">"))
        {
            return int.TryParse(requirement[1..], out int threshold) && value > threshold;
        }
        else if (requirement.StartsWith("<"))
        {
            return int.TryParse(requirement[1..], out int threshold) && value < threshold;
        }
        return true;
    }

    static void ShowEnding(Ending ending, int decisionsCount)
    {
        System.Console.Clear();
        System.Console.ForegroundColor = ConsoleColor.Magenta;
        System.Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════╗");
        System.Console.WriteLine("║                            GAME OVER - ENDING                              ║");
        System.Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════╝");
        System.Console.ResetColor();
        System.Console.WriteLine();

        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine(ending.Title);
        System.Console.ResetColor();
        System.Console.WriteLine();
        System.Console.WriteLine(WrapText(ending.Description, 76));
        System.Console.WriteLine();

        if (ending.Consequences.Count > 0)
        {
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("Consequences:");
            foreach (var consequence in ending.Consequences)
            {
                System.Console.WriteLine($"  • {consequence}");
            }
            System.Console.ResetColor();
        }

        System.Console.WriteLine();
        System.Console.WriteLine($"Total Decisions Made: {decisionsCount}");
        DrawResources();

        System.Console.WriteLine("\nPress any key to exit...");
        System.Console.CursorVisible = true;
        System.Console.ReadKey();
    }

    static void ShowGameOver(List<Ending> endings, int decisionsCount)
    {
        var gameOverEnding = endings.FirstOrDefault(e => e.Title.Contains("Disaster") || e.Title.Contains("Fail")) 
                           ?? endings[0];
        ShowEnding(gameOverEnding, decisionsCount);
    }

    static string WrapText(string text, int width)
    {
        if (string.IsNullOrEmpty(text)) return "";

        var words = text.Split(' ');
        var lines = new List<string>();
        var currentLine = "";

        foreach (var word in words)
        {
            if ((currentLine + " " + word).Length > width)
            {
                lines.Add(currentLine);
                currentLine = word;
            }
            else
            {
                currentLine = (currentLine + " " + word).Trim();
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine);
        }

        return string.Join(Environment.NewLine, lines);
    }
}
