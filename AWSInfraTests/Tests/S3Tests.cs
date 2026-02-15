using AWSInfraTests.Base;
using AWSInfraTests.Config;
using AWSInfraTests.Services;

namespace AWSInfraTests.Tests
{
    [TestFixture]
    public class S3Tests : TestBase
    {
        private S3Service _s3Service;
        private string _testBucketName;

        [SetUp]
        public void S3Setup()
        {
            _s3Service = new S3Service(Credentials, Region);
            _testBucketName = $"{TestConfiguration.TestBucketPrefix}-{DateTime.UtcNow:yyyyMMdd-HHmmss}";
        }

        [TearDown]
        public async Task S3TearDown()
        {
            try
            {
                if (await _s3Service.BucketExistsAsync(_testBucketName))
                {
                    var objects = await _s3Service.ListObjectsAsync(_testBucketName);
                    if (objects != null)
                    {
                        foreach (var obj in objects)
                        {
                            await _s3Service.DeleteObjectAsync(_testBucketName, obj.Key);
                            TestContext.Out.WriteLine($"Deleted object: {obj.Key}");
                        }
                    }
                    
                    await _s3Service.DeleteBucketAsync(_testBucketName);
                    TestContext.Out.WriteLine($"Cleaned up bucket: {_testBucketName}");
                }
            }
            catch (Exception ex)
            {
                TestContext.Out.WriteLine($"Cleanup warning: {ex.Message}");
            }
        }

        [Test]
        [Category("S3")]
        public async Task Test_Bucket_Creation_And_Existence()
        {
            var response = await _s3Service.CreateTestBucketAsync(_testBucketName);
            var exists = await _s3Service.BucketExistsAsync(_testBucketName);

            Assert.That(response.HttpStatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            Assert.That(exists, Is.True);
        }

        [Test]
        [Category("S3")]
        public async Task Test_File_Upload_And_Listing()
        {
            await _s3Service.CreateTestBucketAsync(_testBucketName);
            
            var uploadSuccess = await _s3Service.UploadTestFileAsync(_testBucketName, "test.txt", "QA test content");
            var objects = await _s3Service.ListObjectsAsync(_testBucketName);

            Assert.That(uploadSuccess, Is.True);
            Assert.That(objects, Has.Count.EqualTo(1));
        }
    }
}