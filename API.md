# Executive Disorder - API Documentation

## Core Systems API

### GameManager

**Purpose:** Central game state management, decision processing, and game flow control.

#### Public Properties
```csharp
static GameManager Instance         // Singleton instance
GameResources resources             // Current resource state
PoliticalCharacter currentCharacter // Selected character
List<DecisionCard> allCards         // All available cards
int decisionsCount                  // Number of decisions made
int maxDecisions                    // Maximum decisions (default: 50)
```

#### Public Methods
```csharp
// Draw next decision card from shuffled deck
DecisionCard DrawCard()

// Process a player decision
void MakeDecision(DecisionCard card, bool isOption1)

// Add a news headline to the feed
void AddNewsHeadline(string headline)

// Add a social media post to the feed
void AddSocialMediaPost(string post)

// Get recent headlines (last 10)
List<string> GetRecentHeadlines()

// Get recent social media posts (last 15)
List<string> GetRecentSocialMedia()

// End the current game and show ending
void EndGame()

// Start a new game
void StartNewGame()

// Return to main menu
void LoadMainMenu()
```

#### Usage Example
```csharp
// Make a decision
DecisionCard card = GameManager.Instance.DrawCard();
GameManager.Instance.MakeDecision(card, true); // Choose option 1

// Check game state
if (GameManager.Instance.resources.IsGameOver())
{
    GameManager.Instance.EndGame();
}
```

---

### GameResources

**Purpose:** Manage the four core resource meters and game over conditions.

#### Public Properties
```csharp
float popularity  // Public approval (0-100)
float stability   // National order (0-100)
float media       // Press support (0-100)
float economic    // Financial health (0-100)
```

#### Public Constants
```csharp
const float MIN_VALUE = 0f
const float MAX_VALUE = 100f
```

#### Public Methods
```csharp
// Modify a specific resource by amount
void ModifyResource(string resourceName, float amount)

// Check if any resource is at 0 (game over)
bool IsGameOver()

// Get ending type based on final resources
string GetEndingType()
```

#### Usage Example
```csharp
// Modify resources
resources.ModifyResource("popularity", 15f);
resources.ModifyResource("economic", -20f);

// Check state
if (resources.IsGameOver())
{
    string ending = resources.GetEndingType();
    Debug.Log($"Game Over: {ending}");
}
```

---

### UIManager

**Purpose:** Handle all UI updates, visual effects, and player interaction.

#### Public Properties
```csharp
// Resource display
Slider popularityBar, stabilityBar, mediaBar, economicBar
TextMeshProUGUI popularityText, stabilityText, mediaText, economicText

// Decision card UI
GameObject cardPanel
TextMeshProUGUI cardTitleText, cardDescriptionText
Image cardImage
Button option1Button, option2Button
TextMeshProUGUI option1Text, option2Text

// News & Social Media
GameObject newsPanel
TextMeshProUGUI newsHeadlineText
GameObject socialMediaPanel
Transform socialMediaContainer

// Visual Effects
Image flashOverlay
ParticleSystem[] effectParticles

// Ending UI
GameObject endingPanel
TextMeshProUGUI endingTitleText, endingDescriptionText, finalStatsText
```

#### Public Methods
```csharp
// Show next decision card
void ShowNextCard()

// Display visual effect by name
void ShowVisualEffect(string effectName)

// Show game ending screen
void ShowEnding(string endingType)

// Button callbacks
void OnPlayAgainButton()
void OnMainMenuButton()
```

#### Usage Example
```csharp
// Show visual effect
UIManager.Instance.ShowVisualEffect("explosion");

// Show ending
UIManager.Instance.ShowEnding("GoldenAge");
```

---

### DecisionCard (ScriptableObject)

**Purpose:** Data structure for decision cards.

#### Public Properties
```csharp
int cardId                           // Unique identifier
string cardTitle                     // Display title
string description                   // Situation description
Sprite cardImage                     // Visual (optional)
string option1Text                   // First choice text
string option2Text                   // Second choice text
DecisionConsequence[] option1Consequences
DecisionConsequence[] option2Consequences
DecisionCard[] cascadeCards          // Follow-up cards
float cascadeProbability             // Cascade chance (0-1)
```

#### Creating Cards
```csharp
// In Unity Editor:
// Right-click > Create > Executive Disorder > Decision Card

// Via Code:
DecisionCard card = ScriptableObject.CreateInstance<DecisionCard>();
card.cardTitle = "Economic Crisis";
card.description = "The market is crashing...";
// Set other properties...
```

---

### DecisionConsequence (Serializable Class)

**Purpose:** Define outcomes of a decision choice.

#### Public Properties
```csharp
string resourceName        // Which resource to affect
float amount              // Change amount (-100 to +100)
string visualEffect       // Effect name ("explosion", "celebration", "crisis")
string audioClip          // Sound to play
string newsHeadline       // Breaking news text
string socialMediaReaction // Social media post
```

#### Example
```csharp
DecisionConsequence consequence = new DecisionConsequence();
consequence.resourceName = "popularity";
consequence.amount = -25f;
consequence.visualEffect = "crisis";
consequence.newsHeadline = "Public Outcry Over Decision";
consequence.socialMediaReaction = "#WorstPresidentEver trending";
```

---

### PoliticalCharacter (ScriptableObject)

**Purpose:** Define playable leader characters.

#### Public Properties
```csharp
string characterName            // Display name
string title                    // Character title/role
Sprite portrait                 // Character image (optional)
string biography                // Background story
string ideology                 // Political ideology
float popularityInfluence       // Multiplier for popularity (0.5-1.5)
float stabilityInfluence        // Multiplier for stability (0.5-1.5)
float mediaInfluence           // Multiplier for media (0.5-1.5)
float economicInfluence        // Multiplier for economic (0.5-1.5)
```

#### Creating Characters
```csharp
// In Unity Editor:
// Right-click > Create > Executive Disorder > Character

// Via Code:
PoliticalCharacter character = ScriptableObject.CreateInstance<PoliticalCharacter>();
character.characterName = "John Doe";
character.title = "The Centrist";
character.ideology = "Moderate";
character.popularityInfluence = 1.0f;
// Set other properties...
```

---

## Scene Controllers

### MainMenuController

**Purpose:** Handle main menu interactions.

#### Public Methods
```csharp
void OnStartGame()              // Start new game
void OnQuitGame()              // Quit application
void SelectCharacter(int index) // Select character by index
```

---

### CrisisController

**Purpose:** Control nuclear crisis opening sequence.

#### Public Properties
```csharp
TextMeshProUGUI crisisTitle
TextMeshProUGUI crisisDescription
Image crisisImage
Button continueButton
float autoProgressDelay  // Auto-advance time (default: 8s)
```

#### Public Methods
```csharp
void OnContinue()  // Progress to gameplay
```

---

## Editor Tools API

### ExecutiveDisorderPipeline

**Purpose:** Content generation and build configuration tool.

#### Menu Location
`Tools > AI Agent > Exec Full Pipeline`

#### Public Methods
```csharp
[MenuItem("Tools/AI Agent/Exec Full Pipeline")]
static void ShowWindow()

void GenerateAllDecisionCards()    // Create 101 cards
void GenerateAllCharacters()       // Create 8 characters
void SetupScenes()                 // Configure scenes
void ConfigureWebGLBuild()        // Optimize build settings
void ExecuteFullPipeline()        // Run all generation
```

#### Customization
```csharp
// Extend the pipeline with custom generation
private void CreateDecisionCard(int id, string title)
{
    // Your custom card generation logic
}

private void CreateCharacter(int id, string name, string title, string ideology)
{
    // Your custom character generation logic
}
```

---

### BuildScript

**Purpose:** Automated build system.

#### Menu Location
`Build > Build WebGL`
`Build > Build All Platforms`

#### Public Methods
```csharp
[MenuItem("Build/Build WebGL")]
static void BuildWebGL()

[MenuItem("Build/Build All Platforms")]
static void BuildAllPlatforms()

// Command line usage
static void BuildFromCommandLine()
```

#### Command Line Build
```bash
Unity -quit -batchmode -projectPath . -executeMethod BuildScript.BuildFromCommandLine
```

---

## Event Flow

### Game Start Sequence
1. `MainMenuController.OnStartGame()`
2. Load "Crisis" scene
3. `CrisisController` shows nuclear crisis
4. Auto-progress or manual continue
5. Load "Gameplay" scene
6. `GameManager.Start()` initializes game
7. `UIManager.ShowNextCard()` displays first decision

### Decision Flow
1. Player clicks option button
2. `UIManager.OnDecisionMade(bool isOption1)`
3. `GameManager.MakeDecision(card, isOption1)`
4. Resources modified via `GameResources.ModifyResource()`
5. Visual effects triggered via `UIManager.ShowVisualEffect()`
6. News/social media updated
7. Cascade check (30% probability)
8. Game over check
9. If not over: `UIManager.ShowNextCard()`
10. If over: `GameManager.EndGame()`

### Cascade Flow
1. Decision consequences applied
2. Random check vs `cascadeProbability`
3. If triggered: Random card from `cascadeCards[]`
4. Insert cascade card at deck position 0
5. Next `DrawCard()` returns cascade
6. Cascade can trigger more cascades

### Game Over Flow
1. Resource reaches 0 OR 50 decisions made
2. `GameManager.EndGame()`
3. `GameResources.GetEndingType()` determines ending
4. Save stats to `PlayerPrefs`
5. `UIManager.ShowEnding(endingType)`
6. Display final stats and ending description
7. Offer "Play Again" or "Main Menu"

---

## Resource Names

### Valid Resource Identifiers
```csharp
"popularity"  // or "Popularity"
"stability"   // or "Stability"
"media"       // or "Media"
"economic"    // or "Economic"
```
*Case insensitive*

---

## Visual Effect Names

### Valid Effect Identifiers
```csharp
"explosion"   // Red flash, boom particles
"celebration" // Gold confetti, happy particles
"crisis"      // Screen shake, dark overlay
```

---

## Ending Types

### Complete List
```csharp
"GoldenAge"      // Avg 80+
"Successful"     // Avg 70-79
"Balanced"       // Avg 60-69
"Struggling"     // Avg 50-59
"Crisis"         // Avg 40-49
"Disaster"       // Avg <40
"Overthrown"     // Popularity = 0
"Collapse"       // Stability = 0
"Censored"       // Media = 0
"Bankrupt"       // Economic = 0
```

---

## Data Persistence

### PlayerPrefs Keys
```csharp
"EndingType"        // string - Last ending achieved
"FinalDecisions"    // int - Decisions made
"FinalPopularity"   // float - Final popularity value
"FinalStability"    // float - Final stability value
"FinalMedia"        // float - Final media value
"FinalEconomic"     // float - Final economic value
```

---

## Extension Points

### Adding New Resources
1. Add property to `GameResources`
2. Update `ModifyResource()` switch
3. Update `IsGameOver()` logic
4. Add UI slider to scenes
5. Update `UIManager.UpdateResourceBars()`

### Adding New Endings
1. Update `GameResources.GetEndingType()`
2. Add cases to `UIManager.GetEndingTitle()`
3. Add cases to `UIManager.GetEndingDescription()`

### Adding New Visual Effects
1. Add particle system to Gameplay scene
2. Add to `UIManager.effectParticles[]`
3. Add case to `UIManager.ShowVisualEffect()`

### Custom Decision Logic
Override `GameManager.MakeDecision()`:
```csharp
public override void MakeDecision(DecisionCard card, bool isOption1)
{
    // Custom pre-processing
    
    base.MakeDecision(card, isOption1);
    
    // Custom post-processing
}
```

---

## Performance Considerations

### Object Pooling
Consider pooling for:
- Social media post UI elements
- Particle effects
- News headline displays

### Memory Management
- Use `Resources.Load<>()` for on-demand loading
- Unload unused assets with `Resources.UnloadUnusedAssets()`
- Keep texture sizes minimal (<512x512)

### WebGL Optimization
- Minimize `PlayerPrefs` usage
- Avoid synchronous `Resources.Load()` in Update
- Use coroutines for animations
- Disable unused Unity modules in Player Settings

---

## Debugging

### Common Issues

**Cards Not Loading:**
```csharp
Debug.Log($"Cards found: {Resources.LoadAll<DecisionCard>("DecisionCards").Length}");
```

**Resource Not Changing:**
```csharp
Debug.Log($"Before: {resources.popularity}, After: {resources.popularity}");
```

**Cascade Not Triggering:**
```csharp
Debug.Log($"Cascade probability: {card.cascadeProbability}, Random: {Random.value}");
```

### Debug Commands
Add to `GameManager`:
```csharp
[ContextMenu("Set All Resources to 100")]
void DebugMaxResources()
{
    resources.popularity = 100;
    resources.stability = 100;
    resources.media = 100;
    resources.economic = 100;
}

[ContextMenu("Force Game Over")]
void DebugGameOver()
{
    resources.popularity = 0;
    EndGame();
}
```

---

## Testing

### Unit Test Examples
```csharp
[Test]
public void TestResourceModification()
{
    GameResources res = new GameResources();
    res.ModifyResource("popularity", 25);
    Assert.AreEqual(75, res.popularity);
}

[Test]
public void TestGameOverCondition()
{
    GameResources res = new GameResources();
    res.popularity = 0;
    Assert.IsTrue(res.IsGameOver());
}

[Test]
public void TestEndingCalculation()
{
    GameResources res = new GameResources();
    res.popularity = 100;
    res.stability = 100;
    res.media = 100;
    res.economic = 100;
    Assert.AreEqual("GoldenAge", res.GetEndingType());
}
```

---

## Version History

### v1.0.0 - Initial Release
- Complete game systems
- 101 decision cards (generated)
- 8 political characters
- 10 unique endings
- WebGL build support
- Nuclear crisis opening
- Consequence cascades
- News & social media systems

---

## Support

For questions or issues:
- Check SETUP.md for installation help
- Review DESIGN.md for game mechanics
- See README.md for overview
- Open GitHub issue for bugs

---

*Executive Disorder API - Make impossible decisions with confidence!* 🎮
