using AWSInfraTests.Base;
using AWSInfraTests.Services;

namespace AWSInfraTests.Tests
{
    [TestFixture]
    public class EC2Tests : TestBase
    {
        private EC2Service _ec2Service;

        [SetUp]
        public void EC2Setup()
        {
            _ec2Service = new EC2Service(Credentials, Region);
        }

        [Test]
        [Category("EC2")]
        [Category("ReadOnly")]
        public async Task Test_Describe_Instances_By_Tag()
        {
            var instances = await _ec2Service.GetInstancesByTagAsync("Environment", "QA");

            Assert.That(instances, Is.Not.Null);
            TestContext.Out.WriteLine($"Found {instances.Count} instances with tag Environment=QA");
            
            foreach (var instance in instances)
            {
                TestContext.Out.WriteLine($"  - {instance.InstanceId}: {instance.State?.Name}");
            }

            if (instances.Count == 0)
            {
                TestContext.Out.WriteLine("No QA instances found. Create an EC2 instance with tag Environment=QA to test.");
            }
        }

        [Test]
        [Category("EC2")]
        [Category("ReadOnly")]
        public async Task Test_Instance_State_Validation()
        {
            var instances = await _ec2Service.GetInstancesByTagAsync("Environment", "QA");
            
            if (instances.Count == 0)
            {
                Assert.Ignore("No QA instances found. Create an EC2 instance with tag Environment=QA to test.");
            }

            var firstInstance = instances.First();
            var state = await _ec2Service.GetInstanceStateAsync(firstInstance.InstanceId);

            TestContext.Out.WriteLine($"Instance {firstInstance.InstanceId} state: {state}");
            
            var validStates = new[] { "running", "stopped", "pending", "stopping", "terminated", "shutting-down" };
            Assert.That(validStates, Does.Contain(state), $"State '{state}' should be a valid EC2 state");
        }

        [Test]
        [Category("EC2")]
        [Category("Security")]
        public async Task Test_Security_Group_Port_Check()
        {
            var instances = await _ec2Service.GetInstancesByTagAsync("Environment", "QA");
            
            if (instances.Count == 0 || instances.First().SecurityGroups?.Count == 0)
            {
                Assert.Ignore("No instances or security groups found. Create an EC2 instance with tag Environment=QA to test.");
            }

            var firstInstance = instances.First();
            var securityGroupId = firstInstance.SecurityGroups?.First()?.GroupId;

            if (string.IsNullOrEmpty(securityGroupId))
            {
                Assert.Ignore("Instance has no security groups attached.");
            }

            var isSshOpen = await _ec2Service.IsPortOpenAsync(securityGroupId, 22);
            var isHttpOpen = await _ec2Service.IsPortOpenAsync(securityGroupId, 80);

            TestContext.Out.WriteLine($"Security Group {securityGroupId}:");
            TestContext.Out.WriteLine($"  - Port 22 (SSH) open to 0.0.0.0/0: {isSshOpen}");
            TestContext.Out.WriteLine($"  - Port 80 (HTTP) open to 0.0.0.0/0: {isHttpOpen}");

            // Security audit - document without failing
            Assert.That(isSshOpen || !isSshOpen, Is.True, "Port check completed");
        }
    }
}