# Enterprise DevSecOps Pipeline with ICS Security Integration

[![🔄 CI Pipeline](https://github.com/papaert/devsecops7-VPS/workflows/%F0%9F%94%84%20CI%20Pipeline%20-%20Development%20Integration/badge.svg)](https://github.com/papaert/devsecops7-VPS/actions)
[![🚀 CD Pipeline](https://github.com/papaert/devsecops7-VPS/workflows/%F0%9F%9A%80%20CD%20Pipeline%20-%20Multi-Platform%20Deployment/badge.svg)](https://github.com/papaert/devsecops7-VPS/actions)
[![SLSA 3](https://slsa.dev/images/gh-badge-level3.svg)](https://slsa.dev)
[![🔒 ICS Compliant](https://img.shields.io/badge/%F0%9F%94%92%20ICS-Compliant-green.svg)](docs/compliance-mapping.md)

## 🏗️ Architecture Overview

Production-grade DevSecOps pipeline integrating Industrial Control Systems (ICS) security principles with modern cloud-native practices.

### 🎯 Key Features

- **🔒 ICS-Grade Security**: Network segmentation, endpoint hardening, runtime protection
- **📋 SBOM Generation**: CycloneDX format with vulnerability correlation
- **🛡️ Multi-Layer Security**: SAST, DAST, SCA, container scanning
- **🏭 Infrastructure as Code**: Terraform + Terragrunt + CloudFormation
- **🔄 Reusable Workflows**: Modular GitHub Actions with environment separation

## 🚀 Quick Start

### Prerequisites
```bash
aws --version          # AWS CLI v2
terraform --version    # Terraform >= 1.6.0
kubectl version        # Kubernetes CLI
docker --version       # Docker Engine
```

### Setup
```bash
git clone https://github.com/papaert/devsecops7-VPS.git
cd devsecops7-VPS
./scripts/bootstrap.sh
```

## 📊 Compliance Framework Alignment

- **SLSA Level 3**: Non-falsifiable provenance
- **SSDF**: Secure Software Development Framework
- **NERC CIP**: Electronic security perimeter
- **IEC 62443**: Security Level 2 (SL-2)

---
**Built with ❤️ for the DevSecOps community**