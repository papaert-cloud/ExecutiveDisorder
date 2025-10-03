# Executive Disorder - Unity Game Project on Replit

## Overview
This is the **Executive Disorder** Unity 6 game project repository, adapted to run on Replit. Since Unity cannot run directly in a cloud environment, this Repl includes a Flask web application that serves as a project documentation and information page.

## Project Information
- **Project Name**: Executive Disorder
- **Type**: Unity 6 Game Project
- **Genre**: Political Decision-Making Card Game
- **Platforms**: Windows, Linux, WebGL
- **Original Repository**: https://github.com/ExecutiveDis/ExecutiveDisorder
- **Current Repository**: https://github.com/papaert-cloud/ExecutiveDisorder

## What's Running on Replit
This Repl runs a complete Flask backend system that:
- **Serves the Unity WebGL game** at the root URL (/)
- **Provides REST API** for user authentication and game saves
- **PostgreSQL database** for user accounts and game progress
- **API documentation page** at /api-docs for testing endpoints
- **Health monitoring** endpoint at /health

The Unity game runs directly in the browser via WebGL, with full backend support for user accounts and save game functionality.

## Repository Structure

### Core Unity Project Files
- **Assets/** - Game assets, scripts, scenes, prefabs, and resources
- **ProjectSettings/** - Unity project configuration files
- **Packages/** - Unity package dependencies

### Web Application (Running on Replit)
- **app/** - Flask web application
  - `app.py` - Main Flask application with routing and CORS configuration
  - `models.py` - SQLAlchemy database models (User, GameSave)
  - `auth_routes.py` - Authentication API endpoints (register, login, logout)
  - `game_routes.py` - Game save API endpoints (save, load, delete)
  - `templates/` - HTML templates (API docs, landing page)
  - `static/` - CSS, JavaScript, and static assets
  - `webgl/` - Unity WebGL build files
  - `requirements.txt` - Python dependencies

### Infrastructure & DevOps
- **kubernetes/** - Kubernetes deployment configurations
- **terraform/** - Infrastructure as Code files
- **scripts/** - Utility and bootstrap scripts
- **docs/** - Additional documentation

## Game Description
Executive Disorder is a political decision-making card game where players:
- Navigate complex political scenarios
- Make decisions that affect multiple resources (popularity, stability, media relations, economy)
- Choose from different political characters with unique attributes
- Experience various media reactions to their decisions
- Reach different endings based on their performance

## Technical Stack

### Unity Project
- **Engine**: Unity 6
- **Language**: C#
- **Data Format**: JSON (characters, cards, endings, media reactions)
- **IDE**: Visual Studio

### Web Application (This Repl)
- **Framework**: Flask 3.0
- **Language**: Python 3.11
- **Database**: PostgreSQL (Replit-hosted)
- **ORM**: SQLAlchemy with Flask-SQLAlchemy
- **Authentication**: Flask-Login with bcrypt password hashing
- **Session Management**: Cookie-based sessions
- **CORS**: Environment-based origin restrictions
- **Server**: Development server (Gunicorn for production)
- **Host**: 0.0.0.0:5000

## Development Workflow

### To Work on Unity Project
1. Clone this repository locally
2. Open the project in Unity 6
3. All game assets and scripts are in the Assets folder
4. Build for Windows, Linux, or WebGL

### To Work on Web App (Replit)
1. The Flask app runs automatically on Replit
2. Edit files in `app/` directory
3. Changes auto-reload in development mode
4. Access via the Replit webview

## Configuration Files

### Python Dependencies
- Flask 3.0.0 - Web framework
- Flask-SQLAlchemy 3.1.1 - Database ORM
- Flask-Login 0.6.3 - User session management
- Flask-Bcrypt 1.0.1 - Password hashing
- Flask-CORS 4.0.0 - Cross-origin resource sharing
- psycopg2-binary 2.9.9 - PostgreSQL adapter
- python-dotenv 1.0.0 - Environment variable management
- Gunicorn 21.2.0 - WSGI server (for production)
- Cryptography 41.0.7 - Security utilities

### Workflow
- **Name**: Flask App
- **Command**: `cd app && python app.py`
- **Port**: 5000
- **Output**: Webview

## Deployment
The Flask application is configured to run on port 5000 with:
- Host: 0.0.0.0 (accessible from all interfaces)
- Debug mode: Enabled (development)
- Cache control: Disabled (for fresh updates)

For production deployment, the app uses Gunicorn as configured in the Dockerfile.

## Getting Started with Unity Project

1. **Clone Repository**
   ```bash
   git clone https://github.com/papaert-cloud/ExecutiveDisorder.git
   ```

2. **Open in Unity Hub**
   - Open Unity Hub
   - Add project from disk
   - Select the root directory
   - Open with Unity 6

3. **Build Settings**
   - Check all required scenes are in "Scenes In Build"
   - For WebGL: Ensure Data Caching is disabled
   - Build for your target platform

4. **Play/Test**
   - Press Play in Unity Editor to test
   - Or build and run the executable

## Game Data
The game includes extensive JSON data files:
- **Characters**: Political characters with unique stats
- **Decision Cards**: Multiple-choice scenarios with consequences
- **Media Reactions**: Dynamic responses to player decisions
- **Endings**: Various outcomes based on resource levels

## Notes
- Unity Library folder is git-ignored (regenerates on project open)
- The web app serves as documentation only
- To play the game, you need to build it from Unity
- WebGL builds can be hosted on any static web server

## Links
- [GitHub Repository](https://github.com/papaert-cloud/ExecutiveDisorder)
- [Original Unity Project](https://github.com/ExecutiveDis/ExecutiveDisorder)

## Backend API Endpoints

### Authentication (`/api/auth`)
- `POST /api/auth/register` - Register new user account
- `POST /api/auth/login` - Login with username/password
- `POST /api/auth/logout` - Logout current user
- `GET /api/auth/check` - Check authentication status
- `GET /api/auth/me` - Get current user info

### Game Saves (`/api/game`)
- `POST /api/game/save` - Create new game save
- `GET /api/game/saves` - Get all saves for current user
- `GET /api/game/save/:id` - Get specific save by ID
- `PUT /api/game/save/:id` - Update existing save
- `DELETE /api/game/save/:id` - Delete a save
- `GET /api/game/stats` - Get gameplay statistics

## Database Schema

### User Model
- `id` (Primary Key)
- `username` (Unique, indexed)
- `email` (Unique, indexed)
- `password_hash` (Bcrypt hashed)
- `created_at` (Timestamp)

### GameSave Model
- `id` (Primary Key)
- `user_id` (Foreign Key → User, cascade delete)
- `character_name` (String)
- `save_data` (JSON - game state)
- `resources` (JSON - popularity, stability, media, economic)
- `decisions_count` (Integer)
- `created_at` (Timestamp)
- `updated_at` (Timestamp, auto-updated)

## Security Features
- **Password Hashing**: Bcrypt with salt
- **Session Management**: Secure cookie-based sessions
- **CORS Protection**: Restricted to environment-configured origins
- **Input Validation**: Type checking and sanitization on all endpoints
- **Error Handling**: Generic error messages with server-side logging
- **Database Safety**: Transaction rollback on all exceptions

## Environment Variables
Required for deployment:
- `DATABASE_URL` - PostgreSQL connection string (auto-provided by Replit)
- `SECRET_KEY` - Flask session secret (auto-generated, change for production)
- `REPLIT_DOMAINS` - Comma-separated allowed CORS origins (auto-provided by Replit)

## Quick Start Guide

### Playing the Game
1. Visit the root URL (/) to load the WebGL game
2. Register an account or login
3. Play the game and your progress auto-saves
4. View your saved games and statistics

### Testing the API
1. Visit `/api-docs` for interactive API documentation
2. Test registration, login, and save game endpoints
3. Use the provided test forms to interact with the API
4. Check `/health` endpoint for system status

## Integration with Unity
The Unity WebGL game can communicate with the backend API using JavaScript:

```javascript
// Example: Register user from Unity
async function RegisterUser(username, email, password) {
    const response = await fetch('/api/auth/register', {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify({username, email, password})
    });
    return await response.json();
}

// Example: Save game progress
async function SaveGame(characterName, saveData, resources, decisionsCount) {
    const response = await fetch('/api/game/save', {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify({
            character_name: characterName,
            save_data: saveData,
            resources: resources,
            decisions_count: decisionsCount
        })
    });
    return await response.json();
}
```

## .NET Applications (.NET 9.0)

### ExecutiveDisorder.Core (Class Library)
- **Models**: Character, DecisionCard, Ending, GameResources, SaveGame
- **Services**: GameDataLoader (JSON deserialization), SaveGameManager (save/load system)
- **Features**: Resource clamping (0-100), game over detection, path resolution

### ExecutiveDisorder.Console (Console Application)
- **Timed Decisions**: 30-second countdown with visual timer, auto-select on timeout
- **Hardened UI**: Console size validation (80x25 min), input validation, error handling
- **Features**: Character selection, decision loop, resource display, recent headlines (3 latest), decision logging, ending detection
- **Save System**: Auto-save to AppData, load on startup, save file management
- **Build**: Self-contained single-file executable for win-x64, linux-x64, osx-x64

### ExecutiveDisorder.Avalonia (GUI Application)
- **Framework**: Avalonia UI 11.2 (cross-platform desktop)
- **Architecture**: MVVM pattern with ViewModels
- **Features**: Full game loop, data binding, Fluent Design theme
- **Platforms**: Windows, macOS, Linux
- **Note**: Designed for local compilation (Replit has GUI limitations)

### ExecutiveDisorder.Tests (Unit Tests)
- **Framework**: xUnit with .NET 9.0
- **Coverage**: 25 tests covering GameResources, GameDataLoader, Models
- **Test Types**: Unit tests, edge cases, malformed data handling
- **Note**: Basic coverage - needs DI and behavioral tests for production

### Solution Structure
```
ExecutiveDisorder.sln
├── ExecutiveDisorder.Core/          # Shared library
├── ExecutiveDisorder.Console/       # Console app
├── ExecutiveDisorder.Avalonia/      # GUI app
└── ExecutiveDisorder.Tests/         # Unit tests
```

## CI/CD Pipeline

### .github/workflows/dotnet-ci.yml
- **Multi-platform builds**: Ubuntu, Windows, macOS
- **Automated testing**: Runs all 25 unit tests
- **Console app releases**: Self-contained single-file executables
- **Avalonia GUI releases**: Cross-platform distributions
- **Code quality**: Formatting verification with dotnet format
- **Triggers**: Push to main/develop, pull requests, releases

### .github/workflows/unity-webgl-deploy.yml
- **Flask validation**: Dependency check and tests
- **JSON validation**: Validates all game data files
- **Asset counting**: Reports cards/characters/endings
- **Triggers**: Push to main (app/** or Assets/** changes)

## Game Content

### Decision Cards
- **Total**: 110 cards (IDs 1-109)
- **New Additions**: Mandatory nap time, national cryptocurrency, robot police force, weather control experiments, etc.

### Characters
- **Total**: 10 playable characters
- **New**: Dr. Nova Synthesis (futuristic tech), Captain Rex Nostalgic (traditionalist)
- **Types**: Progressive, Conservative, Populist, Technocrat, Environmentalist, etc.

### Endings
- **Total**: 12 different endings
- **New**: "The Meme Presidency", "The Quantum Paradox"
- **Types**: Victory, Disaster, Chaos, Utopia, etc.

## Last Updated
October 3, 2025

## Recent Changes
- **October 3, 2025 (Latest)**: Complete .NET ecosystem implementation
  - Created ExecutiveDisorder.Core library with models and services
  - Built ExecutiveDisorder.Console with timed decisions and save system
  - Developed ExecutiveDisorder.Avalonia GUI with MVVM architecture
  - Added ExecutiveDisorder.Tests with 25 unit tests
  - Set up GitHub Actions CI/CD for automated builds and releases
  - Added 10 new Unity decision cards (IDs 100-109)
  - Created 2 new characters (Dr. Nova Synthesis, Captain Rex Nostalgic)
  - Added 2 new endings ("The Meme Presidency", "The Quantum Paradox")

- **October 3, 2025**: Implemented complete backend system
  - Added PostgreSQL database with User and GameSave models
  - Built REST API for authentication and game saves
  - Deployed Unity WebGL build at root URL
  - Created interactive API documentation page
  - Implemented secure session management and password hashing
  - Added CORS protection and comprehensive error handling
