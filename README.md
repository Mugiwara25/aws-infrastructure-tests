# 🧪 AWS Infrastructure Testing Suite

A professional automation testing suite for AWS infrastructure services using C#, AWS SDK, and NUnit. Demonstrates cloud resource validation, automated testing methodologies, and infrastructure-as-code quality assurance.

## 🚀 Repository
[View Project](https://github.com/Mugiwara25/aws-infrastructure-tests)

## 📊 Test Coverage
- **Amazon S3** - Automated bucket lifecycle testing (creation, upload, cleanup)
- **Amazon EC2** - Instance state validation, tag queries, security group audits
- **Total Tests** - 5/5 passing with automated resource cleanup

## 🛠️ Technologies Used
- C# (.NET 9)
- NUnit 4
- AWS SDK (S3, EC2)
- GitHub Actions (CI/CD ready)

## 📁 Project Structure
AWSInfrastructureTests/
├── AWSInfraTests/
│   ├── Base/
│   │   └── TestBase.cs              # Base test class with AWS credentials
│   ├── Config/
│   │   └── TestConfiguration.cs     # Test settings and region config
│   ├── Services/
│   │   ├── S3Service.cs             # S3 operations wrapper
│   │   └── EC2Service.cs            # EC2 operations wrapper
│   ├── Tests/
│   │   ├── S3Tests.cs               # S3 bucket lifecycle tests
│   │   └── EC2Tests.cs              # EC2 instance validation tests
│   └── AWSInfraTests.csproj
└── README.md


## 🎯 Test Categories

### S3 Tests
| Test | Description | Status |
|------|-------------|--------|
| `Test_Bucket_Creation_And_Existence` | Validates bucket creation and existence | ✅ Pass |
| `Test_File_Upload_And_Listing` | Tests file upload, listing, cleanup | ✅ Pass |

### EC2 Tests
| Test | Description | Status |
|------|-------------|--------|
| `Test_Describe_Instances_By_Tag` | Queries instances by Environment=QA tag | ✅ Pass |
| `Test_Instance_State_Validation` | Validates instance state (running/stopped) | ✅ Pass |
| `Test_Security_Group_Port_Check` | Audits security group rules (SSH/HTTP) | ✅ Pass |

## 🔧 Key Features
- **Automated Resource Cleanup** - All test resources deleted after execution
- **Multi-Region Support** - Configurable AWS regions (default: eu-north-1)
- **Credential Management** - AWS credential chain integration
- **Error Handling** - Graceful handling of missing resources
- **Security First** - Read-only EC2 tests, no hardcoded secrets

## 🚀 Quick Start

### Prerequisites
- AWS account with access keys
- .NET 9 SDK
- EC2 instance tagged `Environment=QA` (for EC2 tests)

### Configuration
1. Configure AWS credentials in `~/.aws/credentials`:
[default]
aws_access_key_id = YOUR_KEY
aws_secret_access_key = YOUR_SECRET

Update region in Config/TestConfiguration.cs if needed

Run Tests
# All tests
dotnet test

# S3 only
dotnet test --filter "Category=S3"

# EC2 only
dotnet test --filter "Category=EC2"

📈 Sample Output
Starting test: Test_Bucket_Creation_And_Existence
Cleaned up bucket: qa-test-bucket-mugiwara25-20260215-092502
Test completed: Passed

Starting test: Test_Security_Group_Port_Check
Security Group sg-0ab5fb9625201ba3c:
  - Port 22 (SSH) open to 0.0.0.0/0: True
  - Port 80 (HTTP) open to 0.0.0.0/0: False
Test completed: Passed

Test summary: total: 5, failed: 0, succeeded: 5, skipped: 0

👨‍💻 Author
Pranav Tamore
Email: pranavtamore25@gmail.com
LinkedIn: linkedin.com/in/pranav-tamore-84a96b21b
GitHub: github.com/Mugiwara25

📄 License
This project is open source and available for educational purposes.