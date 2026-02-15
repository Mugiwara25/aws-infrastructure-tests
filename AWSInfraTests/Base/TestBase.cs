using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using NUnit.Framework;
using AWSInfraTests.Config;
#pragma warning disable CS8618
namespace AWSInfraTests.Base
{
    public class TestBase
    {
        protected static AWSCredentials Credentials;
        protected static RegionEndpoint Region;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var credPath = Path.Combine(home, ".aws", "credentials");
            
            TestContext.Out.WriteLine($"=== DEBUG ===");
            TestContext.Out.WriteLine($"Home directory: {home}");
            TestContext.Out.WriteLine($"Looking for: {credPath}");
            TestContext.Out.WriteLine($"File exists: {File.Exists(credPath)}");
            
            if (File.Exists(credPath))
            {
                TestContext.Out.WriteLine($"File size: {new FileInfo(credPath).Length} bytes");
                TestContext.Out.WriteLine($"First line: {File.ReadLines(credPath).FirstOrDefault()}");
            }
            else
            {
                // List all files in .aws folder
                var awsFolder = Path.Combine(home, ".aws");
                if (Directory.Exists(awsFolder))
                {
                    TestContext.Out.WriteLine($"Contents of {awsFolder}:");
                    foreach (var f in Directory.GetFiles(awsFolder))
                    {
                        TestContext.Out.WriteLine($"  - {Path.GetFileName(f)}");
                    }
                }
                else
                {
                    TestContext.Out.WriteLine($".aws folder does not exist!");
                }
                
                throw new FileNotFoundException($"Credentials not found at: {credPath}");
            }

            // Load credentials explicitly
            var chain = new CredentialProfileStoreChain(credPath);
            if (!chain.TryGetAWSCredentials("default", out Credentials))
            {
                throw new Exception("Could not load 'default' profile from credentials file");
            }

            Region = RegionEndpoint.GetBySystemName(TestConfiguration.Region);
            TestContext.Out.WriteLine($"Credentials loaded successfully!");
            TestContext.Out.WriteLine($"=== END DEBUG ===");
        }

        [SetUp]
        public void Setup()
        {
            TestContext.Out.WriteLine($"Starting test: {TestContext.CurrentContext.Test.Name}");
        }

        [TearDown]
        public void TearDown()
        {
            var result = TestContext.CurrentContext.Result.Outcome.Status;
            TestContext.Out.WriteLine($"Test completed: {result}");
        }
    }
}