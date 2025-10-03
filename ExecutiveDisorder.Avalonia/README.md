# Executive Disorder - Avalonia GUI

Cross-platform desktop GUI version of Executive Disorder built with Avalonia UI.

## Features

- **Cross-platform**: Runs on Windows, macOS, and Linux
- **Modern UI**: Built with Avalonia 11.2 and Fluent Design
- **Game Data Integration**: Uses the same JSON files as the Unity WebGL version
- **Full Gameplay**: Character selection, decision cards, resource management, and endings

## Project Structure

```
ExecutiveDisorder.Avalonia/
├── Models/                    # Data models for JSON deserialization
│   ├── Character.cs          # Character model
│   ├── DecisionCard.cs       # Card and choice models
│   └── Ending.cs             # Game ending models
├── ViewModels/                # MVVM ViewModels
│   └── MainWindowViewModel.cs # Main game logic
├── MainWindow.axaml          # Main UI layout
├── MainWindow.axaml.cs       # Main window code-behind
├── Program.cs                # Application entry point
├── App.axaml                 # Application styles
└── App.axaml.cs              # Application initialization
```

## Requirements

- .NET 9.0 SDK or later
- Windows 10/11, macOS 10.15+, or modern Linux distribution

## Building Locally

### 1. Install .NET 9 SDK

Download from: https://dotnet.microsoft.com/download/dotnet/9.0

### 2. Clone the Repository

```bash
git clone https://github.com/papaert-cloud/ExecutiveDisorder.git
cd ExecutiveDisorder/ExecutiveDisorder.Avalonia
```

### 3. Restore NuGet Packages

```bash
dotnet restore
```

### 4. Build the Project

```bash
dotnet build
```

### 5. Run the Application

```bash
dotnet run
```

## Publishing for Distribution

### Windows

```bash
dotnet publish -c Release -r win-x64 --self-contained
```

### macOS

```bash
dotnet publish -c Release -r osx-x64 --self-contained
```

### Linux

```bash
dotnet publish -c Release -r linux-x64 --self-contained
```

## Game Data

The application reads from three JSON files in the `Data/` directory:

- `cardsjson.json` - 110 decision cards
- `charactersjson.json` - 10 political characters
- `endingjson.json` - 12 game endings

These files are automatically copied from `Assets/` during build.

## How to Play

1. **Select a Character**: Choose from 10 political archetypes, each with unique starting resources
2. **Make Decisions**: Read scenarios and choose from multiple options
3. **Manage Resources**: Track Popularity, Stability, Media Trust, and Economic Health
4. **Reach an Ending**: Achieve one of 12 different endings based on your choices

## Architecture

- **MVVM Pattern**: Clean separation of UI and logic
- **Reactive UI**: Built with Avalonia.ReactiveUI
- **Data Binding**: Automatic UI updates with INotifyPropertyChanged
- **JSON Deserialization**: System.Text.Json for game data loading

## Dependencies

```xml
<PackageReference Include="Avalonia" Version="11.2.0" />
<PackageReference Include="Avalonia.Desktop" Version="11.2.0" />
<PackageReference Include="Avalonia.Themes.Fluent" Version="11.2.0" />
<PackageReference Include="Avalonia.Fonts.Inter" Version="11.2.0" />
<PackageReference Include="Avalonia.ReactiveUI" Version="11.2.0" />
<PackageReference Include="System.Text.Json" Version="9.0.0" />
```

## Troubleshooting

### Build Errors

If you encounter build errors:

1. Ensure .NET 9 SDK is installed: `dotnet --version`
2. Clean the project: `dotnet clean`
3. Restore packages: `dotnet restore`
4. Rebuild: `dotnet build`

### Missing Data Files

If the game can't find JSON data:

1. Check that `Assets/cardsjson.json`, `Assets/charactersjson.json`, and `Assets/endingjson.json` exist in the repository root
2. The `.csproj` file automatically copies these to `Data/` folder during build

### Platform-Specific Issues

**Linux**: Install required dependencies:
```bash
sudo apt-get install -y libx11-dev libice-dev libsm-dev
```

**macOS**: May need to allow the app in Security & Privacy settings

## Development Notes

This GUI was designed to:
- Provide a native desktop experience for Executive Disorder
- Reuse the Unity game data (JSON files)
- Work cross-platform using Avalonia UI framework
- Follow MVVM architectural pattern

## Future Enhancements

- Save/load game state
- Achievements tracking
- Sound effects and music
- Animations and transitions
- Multiplayer support
- Modding/custom scenario support

---

**Built with Avalonia UI** | **Democracy: Optional. Chaos: Guaranteed.**
