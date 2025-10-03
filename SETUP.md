# Executive Disorder - Setup Guide

## Quick Start

### 1. Opening the Project
1. Install Unity Hub if not already installed
2. Install Unity 6.0.23f1 or later
3. Clone this repository:
   ```bash
   git clone https://github.com/papaert-cloud/ExecutiveDisorder.git
   ```
4. Open Unity Hub and click "Add" → Select the `ExecutiveDisorder` folder
5. Open the project (Unity will import all packages)

### 2. Generate Game Content

The project includes an automated pipeline to generate all game content:

#### Using the AI Agent Pipeline
1. Open Unity
2. Go to menu: **Tools > AI Agent > Exec Full Pipeline**
3. A window will open with several options:

   ![Pipeline Window]

   **Available Actions:**
   - **Generate All Decision Cards (101)**: Creates all decision cards automatically
   - **Generate All Characters (8)**: Creates 8 unique political characters
   - **Setup All Scenes**: Configures all game scenes
   - **Configure WebGL Build**: Optimizes build settings for <50MB
   - **Execute Full Pipeline**: Runs all actions above in sequence

4. Click **"Execute Full Pipeline"** to generate everything
5. Wait for completion message

#### What Gets Generated:
- ✅ 101 Decision Cards in `Assets/Resources/DecisionCards/`
- ✅ 8 Political Characters in `Assets/Resources/Characters/`
- ✅ 3 Configured Scenes (MainMenu, Crisis, Gameplay)
- ✅ WebGL Build Settings optimized

### 3. Testing in Unity Editor

1. **Test Main Menu Scene:**
   - Open `Assets/Scenes/MainMenu.unity`
   - Press Play (▶️)
   - Click "Start Game"

2. **Test Crisis Opening:**
   - Should automatically load after menu
   - Shows nuclear crisis introduction
   - Auto-progresses or click Continue

3. **Test Gameplay:**
   - Make decisions by clicking options
   - Watch resource meters change
   - See news headlines and social media reactions
   - Play until game over or 50 decisions

### 4. Building for WebGL

#### Method 1: Using Unity UI
1. File > Build Settings
2. Select "WebGL" platform
3. Click "Switch Platform" (if not already selected)
4. Click "Build"
5. Choose output folder
6. Wait for build to complete

#### Method 2: Using Command Line
```bash
# From Unity project root
Unity -quit -batchmode -projectPath . -executeMethod BuildScript.BuildWebGL
```

#### Build Output:
- Total size: <50MB (optimized)
- Compression: Brotli enabled
- Memory: 256MB allocated
- Compatible with modern browsers

### 5. Running the Built Game

1. Locate your build folder
2. Start a local web server:
   ```bash
   # Python 3
   python -m http.server 8000
   
   # Python 2
   python -m SimpleHTTPServer 8000
   
   # Node.js
   npx http-server
   ```
3. Open browser: `http://localhost:8000`
4. Navigate to build folder and open `index.html`

### 6. Customizing Content

#### Adding Custom Decision Cards
1. Right-click in Project window
2. Create > Executive Disorder > Decision Card
3. Fill in properties:
   - Card Title
   - Description
   - Option 1 & 2 Text
   - Consequences (resources, effects, headlines)
4. Save in `Assets/Resources/DecisionCards/`

#### Creating Custom Characters
1. Right-click in Project window
2. Create > Executive Disorder > Character
3. Fill in properties:
   - Name and Title
   - Biography
   - Ideology
   - Influence modifiers
4. Save in `Assets/Resources/Characters/`

### 7. Troubleshooting

#### Unity Can't Find TextMeshPro
- Window > Package Manager
- Search "TextMeshPro"
- Click "Install"

#### Build Size Too Large
- Check Project Settings > Player > WebGL
- Ensure compression is enabled
- Reduce texture quality in quality settings
- Remove unused assets

#### Decision Cards Not Loading
- Ensure cards are in `Assets/Resources/DecisionCards/`
- Check card naming follows pattern: `Card_XXX_Name.asset`
- Verify cards have proper `.meta` files

#### Game Crashes on Decision
- Check all consequence arrays are initialized
- Verify resource names are correct (popularity, stability, media, economic)
- Check for null references in UIManager

#### WebGL Build Won't Run
- Use a web server (not file:// protocol)
- Check browser console for errors
- Ensure browser supports WebGL 2.0
- Try different browser (Chrome/Firefox recommended)

### 8. Development Workflow

#### Recommended Setup
1. Keep Unity Editor open
2. Make changes to scripts in your preferred IDE
3. Unity auto-recompiles on save
4. Test immediately in Play mode
5. Use Pipeline tool to regenerate content as needed

#### Testing Cycle
1. Make code changes
2. Save files
3. Wait for Unity to compile
4. Press Play in Editor
5. Test functionality
6. Stop Play mode
7. Repeat

### 9. Project Structure Explained

```
ExecutiveDisorder/
├── Assets/
│   ├── Editor/                    # Editor tools
│   │   └── ExecutiveDisorderPipeline.cs
│   ├── Scenes/                    # Game scenes
│   │   ├── MainMenu.unity
│   │   ├── Crisis.unity
│   │   └── Gameplay.unity
│   ├── Scripts/                   # Game logic
│   │   ├── GameManager.cs         # Core game state
│   │   ├── UIManager.cs           # UI control
│   │   ├── GameResources.cs       # Resource system
│   │   ├── DecisionCard.cs        # Card data
│   │   └── ...
│   ├── Resources/                 # Runtime loadable assets
│   │   ├── DecisionCards/         # Generated cards
│   │   └── Characters/            # Generated characters
│   └── Prefabs/                   # Reusable objects
├── Packages/                      # Unity packages
├── ProjectSettings/               # Unity configuration
└── README.md                      # Documentation
```

### 10. Next Steps

After setup:
1. ✅ Run the pipeline to generate content
2. ✅ Play the game in editor
3. ✅ Customize decision cards
4. ✅ Add your own characters
5. ✅ Build for WebGL
6. ✅ Deploy to web hosting

### 11. Deployment Options

#### GitHub Pages
1. Build for WebGL
2. Create `docs/` folder in repo
3. Copy build files to `docs/`
4. Enable GitHub Pages in repo settings
5. Set source to `docs/` folder

#### itch.io
1. Build for WebGL
2. Create itch.io account
3. Create new project
4. Upload ZIP of build folder
5. Set as WebGL/HTML5 project

#### Netlify/Vercel
1. Build for WebGL
2. Create account
3. Drag-drop build folder
4. Get instant URL

### 12. Performance Tips

- Keep decision card descriptions concise
- Use compressed textures for images
- Limit particle effects
- Pool UI elements instead of destroying
- Use object pooling for social media posts
- Minimize draw calls with atlasing

### 13. Support & Resources

- **Unity Documentation**: https://docs.unity3d.com
- **TextMeshPro Guide**: https://docs.unity3d.com/Packages/com.unity.textmeshpro@latest
- **WebGL Best Practices**: https://docs.unity3d.com/Manual/webgl-building.html

### 14. Known Limitations

- ⚠️ WebGL builds don't support threading
- ⚠️ Some browsers limit memory usage
- ⚠️ Mobile support may vary
- ⚠️ Audio may require user interaction to start

---

## Ready to Create Chaos?

Your satirical political simulator is ready to go! Use the pipeline tool to generate content and start making impossible decisions. Every choice matters... or does it? 🎭🏛️

Happy governing! (Or not!) 😄
