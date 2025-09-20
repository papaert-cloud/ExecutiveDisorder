# GitHub Repository Secrets Setup

## Required Secrets

Navigate to: `Settings > Secrets and variables > Actions`

### Security Scanning
```
SNYK_TOKEN=your_snyk_api_token
```

### AWS Integration
```
AWS_ROLE_ARN=arn:aws:iam::ACCOUNT:role/GitHubActionsRole
AWS_REGION=us-east-1
```

### Container Registry
```
DOCKER_USERNAME=your_docker_username
DOCKER_PASSWORD=your_docker_token
```

## Setup Commands

```bash
# Add secrets via GitHub CLI
gh secret set SNYK_TOKEN --body "your_snyk_token"
gh secret set AWS_ROLE_ARN --body "arn:aws:iam::ACCOUNT:role/GitHubActionsRole"
gh secret set AWS_REGION --body "us-east-1"
```