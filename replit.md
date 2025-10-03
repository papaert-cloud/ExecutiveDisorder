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
This Repl runs a Flask web application (Python) that:
- Provides comprehensive project documentation
- Displays game features and technical information
- Shows repository structure and setup instructions
- Serves as a landing page for the Unity project

The actual Unity game cannot run in Replit as it requires Unity Editor for development and building.

## Repository Structure

### Core Unity Project Files
- **Assets/** - Game assets, scripts, scenes, prefabs, and resources
- **ProjectSettings/** - Unity project configuration files
- **Packages/** - Unity package dependencies

### Web Application (Running on Replit)
- **app/** - Flask web application
  - `app.py` - Main Flask application
  - `templates/` - HTML templates
  - `static/` - CSS, JavaScript, and static assets
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

## Last Updated
October 3, 2025
