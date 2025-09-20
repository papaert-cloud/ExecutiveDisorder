# enterprise-devsecops-superlab
🏭 Enterprise-Grade DevSecOps Super-Laboratory: Production-ready security pipeline with ICS security controls, shift-left security, and comprehensive SBOM/vulnerability management using AWS, Terraform, Terragrunt, and GitHub Actions

## Code Quality Enforcement

This repository enforces End-of-Line (EOL) standards and code quality through automated GitHub Actions workflows:

- **EOL Checking**: Automatically validates that all files use proper LF line endings (no CRLF)
- **Whitespace Validation**: Detects and prevents trailing whitespace in source files
- **EditorConfig Compliance**: Ensures consistent formatting across different editors and IDEs

The EOL check runs on every push and pull request to `main` and `develop` branches, helping maintain consistent code quality across the enterprise DevSecOps pipeline.
