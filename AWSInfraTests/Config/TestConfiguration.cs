namespace AWSInfraTests.Config
{
    public static class TestConfiguration
    {
        public const string Region = "eu-north-1";  // Changed from us-east-1
        public const string TestBucketPrefix = "qa-test-bucket-mugiwara25";
        public const string TestInstanceTag = "QA-Test-Instance";
        public const int DefaultTimeoutSeconds = 30;
    }
}