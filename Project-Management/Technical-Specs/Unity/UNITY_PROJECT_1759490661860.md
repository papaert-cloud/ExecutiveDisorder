# Executive Disorder - Unity 6 Game Project

## Project Structure

This repository contains the **Executive Disorder** Unity 6 game project files.

### Unity Project Files

```
ExecutiveDisorderReplit/
├── Assets/                    # Unity game assets
│   ├── Art/                  # Game artwork and sprites
│   ├── Audio/                # Sound effects and music
│   ├── Data/                 # Game data files (JSON)
│   ├── Materials/            # Unity materials
│   ├── Plugins/              # Third-party plugins
│   ├── Prefabs/              # Unity prefabs
│   ├── Resources/            # Runtime resources
│   ├── Scenes/               # Unity scenes
│   ├── Scripts/              # C# game scripts
│   ├── Settings/             # Game settings
│   ├── Shaders/              # Custom shaders
│   └── TextMesh Pro/         # TextMeshPro assets
├── Packages/                 # Unity package dependencies
├── ProjectSettings/          # Unity project settings
├── .vsconfig                 # Visual Studio configuration
└── ExecutiveDisord/          # Original project folder
    └── My project (1)/       # Unity project instance
```

### Game Data Files

Located in `Assets/`:
- `cardsjson.json` - Card game data
- `charactersjson.json` - Character definitions
- `download.json` - Download configuration
- `endingjson.json` - Game ending scenarios

### Development Setup

1. **Unity Version**: Unity 6 (latest)
2. **IDE**: Visual Studio (configured via `.vsconfig`)
3. **Platform**: Windows, Linux, WebGL

### Opening the Project

#### Option 1: Main Unity Project
```bash
# Open with Unity Hub
unity-hub --projectPath "C:\Users\POK28\source\repos\ExecutiveDisorderReplit"
```

#### Option 2: Legacy Project Location
```bash
# Open the legacy project
unity-hub --projectPath "C:\Users\POK28\source\repos\ExecutiveDisorderReplit\ExecutiveDisord\My project (1)"
```

### Build Information

- **Windows Build**: `ExecutiveDisord/My project (1)/BuildWindows/`
- **Build Archive**: `BuildWindows.rar` (available in cloned source)

### Git Configuration

#### Remotes
- **origin**: `git@github.com:papaert-cloud/ExecutiveDisorder.git`
- **upstream**: `git@github.com:papaert-cloud/ExecutiveDisorder.git`

#### Sync Command
```bash
# Use the sync script to push/pull from both remotes
./scripts/sync-repos.sh
# or
git sync
```

### Source Repository

Original Unity project cloned from: `https://github.com/ExecutiveDis/ExecutiveDisorder.git`

### Infrastructure Files

This repository also includes:
- **Terraform**: Infrastructure as Code (`terraform/`)
- **Kubernetes**: Deployment configs (`kubernetes/`)
- **Docker**: Container configuration (`app/`)
- **Scripts**: Bootstrap and sync scripts (`scripts/`)

### Notes

- Unity Library folder is ignored (will regenerate on project open)
- Large binary files (BuildWindows.rar - 48MB) are in the source repo
- Use `.gitignore` to exclude generated Unity files

## Next Steps

1. Open the project in Unity 6
2. Generate WebGL build for deployment
3. Set up CI/CD pipeline for automated builds
4. Deploy to web hosting platform

---
**Project**: Executive Disorder  
**Engine**: Unity 6  
**Repository**: https://github.com/papaert-cloud/ExecutiveDisorder  
**Last Updated**: October 3, 2025
