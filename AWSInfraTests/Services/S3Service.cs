using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace AWSInfraTests.Services
{
    public class S3Service
    {
        private readonly AmazonS3Client _client;

        public S3Service(AWSCredentials credentials, RegionEndpoint region)
        {
            _client = new AmazonS3Client(credentials, region);
        }

        public async Task<bool> BucketExistsAsync(string bucketName)
        {
            try
            {
                var response = await _client.ListBucketsAsync();
                return response.Buckets.Any(b => b.BucketName == bucketName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking bucket: {ex.Message}");
                return false;
            }
        }

        public async Task<PutBucketResponse> CreateTestBucketAsync(string bucketName)
        {
            var request = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };
            
            return await _client.PutBucketAsync(request);
        }

        public async Task<DeleteBucketResponse> DeleteBucketAsync(string bucketName)
        {
            return await _client.DeleteBucketAsync(bucketName);
        }

        public async Task<bool> UploadTestFileAsync(string bucketName, string key, string content)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    ContentBody = content
                };
                
                var response = await _client.PutObjectAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload failed: {ex.Message}");
                return false;
            }
        }

        public async Task<List<S3Object>> ListObjectsAsync(string bucketName)
        {
            try
            {
                var response = await _client.ListObjectsAsync(bucketName);
                return response.S3Objects;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing objects: {ex.Message}");
                return new List<S3Object>();
            }
        }

        public async Task DeleteObjectAsync(string bucketName, string key)
        {
            await _client.DeleteObjectAsync(bucketName, key);
        }
    }
}