using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Runtime;

namespace AWSInfraTests.Services
{
    public class EC2Service
    {
        private readonly AmazonEC2Client _client;

        public EC2Service(AWSCredentials credentials, RegionEndpoint region)
        {
            _client = new AmazonEC2Client(credentials, region);
        }

        public async Task<List<Instance>> GetInstancesByTagAsync(string tagKey, string tagValue)
        {
            var request = new DescribeInstancesRequest
            {
                Filters = new List<Filter>
                {
                    new Filter
                    {
                        Name = $"tag:{tagKey}",
                        Values = new List<string> { tagValue }
                    }
                }
            };

            var response = await _client.DescribeInstancesAsync(request);
            
            // FIX: Handle null reservations
            if (response.Reservations == null)
            {
                return new List<Instance>();
            }

            return response.Reservations
                .SelectMany(r => r.Instances ?? new List<Instance>())
                .ToList();
        }

        public async Task<string> GetInstanceStateAsync(string instanceId)
        {
            var request = new DescribeInstancesRequest
            {
                InstanceIds = new List<string> { instanceId }
            };

            var response = await _client.DescribeInstancesAsync(request);
            var instance = response.Reservations?.FirstOrDefault()?.Instances?.FirstOrDefault();
            
            return instance?.State?.Name ?? "unknown";
        }

        public async Task<List<SecurityGroup>> GetSecurityGroupsAsync(List<string> groupIds)
        {
            var request = new DescribeSecurityGroupsRequest
            {
                GroupIds = groupIds
            };

            var response = await _client.DescribeSecurityGroupsAsync(request);
            return response.SecurityGroups ?? new List<SecurityGroup>();
        }

        public async Task<bool> IsPortOpenAsync(string groupId, int port, string protocol = "tcp")
        {
            var groups = await GetSecurityGroupsAsync(new List<string> { groupId });
            var group = groups.FirstOrDefault();

            if (group == null) return false;

            var permissions = group.IpPermissions ?? new List<IpPermission>();
            
            return permissions.Any(p => 
                p.IpProtocol == protocol &&
                p.FromPort <= port &&
                p.ToPort >= port &&
                (p.Ipv4Ranges?.Any(ip => ip.CidrIp == "0.0.0.0/0") ?? false)
            );
        }
    }
}