#!/bin/bash

# Two-way sync script for ExecutiveDisorder repositories
# Keeps origin (papaert-cloud/ExecutiveDisorder) and upstream (ExecutiveDis/COPY) in sync

set -e

echo "🔄 Starting two-way repository sync..."
echo "=================================="

# Get current branch
CURRENT_BRANCH=$(git branch --show-current)
echo "📍 Current branch: $CURRENT_BRANCH"

# Fetch from both remotes
echo ""
echo "📥 Fetching from origin (papaert-cloud/ExecutiveDisorder)..."
git fetch origin

echo ""
echo "📥 Fetching from upstream (ExecutiveDis/COPY)..."
git fetch upstream

# Pull latest changes from origin
echo ""
echo "⬇️  Pulling from origin/$CURRENT_BRANCH..."
git pull origin $CURRENT_BRANCH --no-rebase --allow-unrelated-histories || {
    echo "⚠️  Failed to pull from origin. Continuing with merge..."
}

# Merge changes from upstream if any
echo ""
echo "🔀 Merging changes from upstream/$CURRENT_BRANCH (if exists)..."
if git show-ref --verify --quiet refs/remotes/upstream/$CURRENT_BRANCH; then
    git merge upstream/$CURRENT_BRANCH --no-edit --allow-unrelated-histories || {
        echo "⚠️  Merge conflict detected. Please resolve conflicts manually."
        exit 1
    }
else
    echo "ℹ️  No upstream/$CURRENT_BRANCH branch found, skipping merge."
fi

# Push to both remotes
echo ""
echo "⬆️  Pushing to origin/$CURRENT_BRANCH..."
git push origin $CURRENT_BRANCH

echo ""
echo "⬆️  Pushing to upstream/$CURRENT_BRANCH..."
git push upstream $CURRENT_BRANCH || {
    echo "⚠️  Failed to push to upstream. You may need to set up push permissions."
    echo "    Make sure you have write access to https://github.com/ExecutiveDis/COPY.git"
    exit 1
}

echo ""
echo "✅ Sync complete! Both repositories are now in sync."
echo "=================================="
