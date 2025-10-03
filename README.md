# ExecutiveDisorder
BAMG introduces Executive Disorder

## 🎮 Satirical Political Decision-Making Game

Executive Disorder is a Unity 6 satirical political game where every decision has dramatic consequences. Navigate crises, manage resources, and try to maintain control as a world leader.

## 🚀 Features

### Core Gameplay
- **3 Immersive Scenes**: MainMenu, Crisis Opening, and Gameplay
- **8 Unique Political Characters**: Each with different ideologies and traits
- **101 Decision Cards**: Every choice impacts your leadership
- **10 Different Endings**: Based on your performance and resource management
- **Nuclear Crisis Opening**: Dramatic start to set the tone

### Dynamic Systems
- **4 Resource Meters**: Popularity, Stability, Media Support, Economic Health
- **Consequence Cascades**: Decisions trigger chain reactions
- **Breaking News Headlines**: Real-time media response to your choices
- **Social Media Reactions**: See how the public responds instantly
- **Visual & Audio Feedback**: Every decision has dramatic effects

### Technical Specs
- **Unity 6.0** (6000.0.23f1)
- **WebGL Build** optimized to <50MB
- **TextMeshPro UI** for crisp text rendering
- **Responsive Design** for various screen sizes

## 📁 Project Structure

```
Assets/
├── Scenes/
│   ├── MainMenu.unity      # Game start screen
│   ├── Crisis.unity        # Nuclear crisis opening
│   └── Gameplay.unity      # Main decision-making gameplay
├── Scripts/
│   ├── GameManager.cs           # Core game logic
│   ├── UIManager.cs             # UI and visual effects
│   ├── GameResources.cs         # Resource management
│   ├── DecisionCard.cs          # Decision card system
│   ├── PoliticalCharacter.cs    # Character definitions
│   ├── MainMenuController.cs    # Menu control
│   └── CrisisController.cs      # Crisis scene logic
├── Editor/
│   └── ExecutiveDisorderPipeline.cs  # Content generation tool
├── Resources/
│   ├── DecisionCards/       # All 101 decision cards (generated)
│   └── Characters/          # All 8 political characters (generated)
└── Prefabs/                 # UI and game prefabs
```

## 🛠️ Setup & Build

### Opening the Project
1. Clone this repository
2. Open with Unity 6.0 or later
3. The project will auto-import all dependencies

### Generating Game Content
Use the built-in AI Agent pipeline:
1. In Unity, go to **Tools > AI Agent > Exec Full Pipeline**
2. Click "Execute Full Pipeline" to generate:
   - All 101 decision cards
   - All 8 political characters
   - Configure WebGL build settings

### Building for WebGL
1. File > Build Settings
2. Select WebGL platform
3. Click "Build" and choose output folder
4. Final build will be <50MB

## 🎯 How to Play

### Game Flow
1. **Main Menu**: Choose your character or start directly
2. **Crisis Opening**: Nuclear crisis sets the urgent tone
3. **Gameplay Loop**: 
   - Read decision card
   - Choose between two options
   - Watch consequences unfold
   - Manage 4 resource meters
   - Survive as long as possible

### Win Conditions
- **Survive 50 decisions**: Make it through your term
- **Maintain Resources**: Keep all meters above 0
- **Multiple Endings**: Based on final resource levels
  - Golden Age (80+ avg)
  - Successful Term (70+ avg)
  - Balanced Leadership (60+ avg)
  - Struggling (50+ avg)
  - Crisis (40+ avg)
  - Disaster (<40 avg)
  - Overthrown (Popularity = 0)
  - Collapse (Stability = 0)
  - Censored (Media = 0)
  - Bankrupt (Economic = 0)

### Resource Management
- **Popularity**: Public approval rating
- **Stability**: National order and security
- **Media**: Press support and communication
- **Economic**: Financial health of the nation

## 🎨 Game Design Philosophy

Executive Disorder satirizes political decision-making by:
- Presenting impossible choices with unintended consequences
- Showing how decisions cascade into unexpected outcomes
- Demonstrating the complexity of leadership through gameplay
- Using humor and exaggeration to highlight real political dynamics

## 🔧 Customization

### Adding New Decisions
1. Create new DecisionCard ScriptableObject
2. Set title, description, and options
3. Define consequences for each choice
4. Add cascade probability and linked cards
5. Place in Resources/DecisionCards/

### Creating Characters
1. Create new PoliticalCharacter ScriptableObject
2. Define name, title, ideology
3. Set influence modifiers
4. Place in Resources/Characters/

### Modifying Endings
Edit `GetEndingTitle()` and `GetEndingDescription()` in `UIManager.cs`

## 📊 Performance Optimization

- Compressed textures for WebGL
- Minimal audio assets
- Efficient UI rendering with object pooling
- Resource streaming for decision cards
- Brotli compression enabled
- Memory limited to 256MB for WebGL

## 🐛 Known Issues & Future Enhancements

### Current Limitations
- Placeholder graphics (will be replaced with custom art)
- Basic audio system (expandable)
- UI requires TextMeshPro setup in scenes

### Planned Features
- Character portraits and animations
- More complex cascade systems
- Multiplayer decision-making
- Historical replay mode
- Achievement system

## 📝 License

This project is created by BAMG. All rights reserved.

## 🤝 Contributing

This is an AI-assisted game development project. The codebase is designed to be:
- Modular and extensible
- Well-documented
- Easy to customize
- Optimized for WebGL deployment

## 🎭 Credits

**Game Design & Development**: BAMG with AI assistance
**Engine**: Unity 6.0
**UI Framework**: TextMeshPro
**Build Target**: WebGL (<50MB)

---

**Executive Disorder** - Where every decision is a crisis, and every crisis is an opportunity... for disaster! 🎮🏛️💥
