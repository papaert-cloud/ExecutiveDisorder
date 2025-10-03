# Repository Sync Setup

## Overview
This repository is configured to sync between two GitHub repositories:
- **Origin**: https://github.com/papaert-cloud/ExecutiveDisorder.git
- **Upstream**: https://github.com/ExecutiveDis/COPY.git

## Configuration

### Git Remotes
```bash
origin    https://github.com/papaert-cloud/ExecutiveDisorder.git
upstream  https://github.com/ExecutiveDis/COPY.git
```

### How to Sync

#### Automatic Sync (Recommended)
Run the sync script to automatically fetch, pull, and push to both repositories:
```bash
./scripts/sync-repos.sh
```

Or use the git alias:
```bash
git sync
```

#### Manual Sync
If you prefer manual control:
```bash
# Fetch from both remotes
git fetch origin
git fetch upstream

# Pull from origin
git pull origin main

# Merge from upstream
git merge upstream/main -X ours --no-edit

# Push to both
git push origin main
git push upstream main
```

## Permissions Setup

### For ExecutiveDis/COPY Repository
To enable push access to the upstream repository, you need:

1. **Personal Access Token (PAT)** with `repo` scope
2. **Collaborator access** to ExecutiveDis/COPY repository

#### Generate a Personal Access Token:
1. Go to GitHub Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Click "Generate new token (classic)"
3. Select scopes: `repo` (all sub-scopes)
4. Generate and copy the token

#### Update Git Credentials:
```bash
# Update the upstream URL with your PAT
git remote set-url upstream https://YOUR_USERNAME:YOUR_TOKEN@github.com/ExecutiveDis/COPY.git
```

Or configure Git credential helper:
```bash
git config --global credential.helper store
```

Then run the sync script again - Git will prompt for credentials and store them.

## Automated Sync with Git Hooks

To automatically sync on every push, add this to `.git/hooks/post-commit`:
```bash
#!/bin/bash
./scripts/sync-repos.sh
```

Make it executable:
```bash
chmod +x .git/hooks/post-commit
```

## Troubleshooting

### Permission Denied
If you get "Permission to ExecutiveDis/COPY.git denied":
- Ensure you have collaborator access to the repository
- Set up your Personal Access Token (see above)
- Contact the repository owner to grant access

### Merge Conflicts
The sync script uses the "ours" merge strategy to automatically resolve conflicts by preferring local changes. If you need manual control:
```bash
git merge --abort  # Abort automatic merge
git merge upstream/main  # Manual merge
# Resolve conflicts manually
git add .
git commit
```

### Force Push
If the upstream repository has diverged significantly:
```bash
git push upstream main --force
```
⚠️ **Warning**: This will overwrite remote history. Use with caution.

## Git Alias Setup

Add to `~/.gitconfig` or run:
```bash
git config --global alias.sync '!bash scripts/sync-repos.sh'
```

Then you can simply run:
```bash
git sync
```
