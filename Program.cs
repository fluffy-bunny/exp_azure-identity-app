using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;

using Microsoft.Extensions.Configuration;

namespace AzIdentityApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddEnvironmentVariables($"{nameof(AzIdentityApp)}_")
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddUserSecrets<Program>()
                .AddCommandLine(args)
                .Build();

            Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", configuration["ClientId"]);
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", configuration["TenantId"]);
            Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", configuration["ClientSecret"]);

            var accountName = configuration["AccountName"];
            var containerName = configuration["ContainerName"];
            var queueName = configuration["QueueName"];

            try
            {
                var blobName = Guid.NewGuid().ToString();
                Console.WriteLine($@"calling CreateBlockBlobAsync(accountName: ""{accountName}"", containerName: ""{containerName}"", bloblName: ""{blobName}"")");
                await BlobTest.CreateBlockBlobAsync(accountName, containerName, blobName);

                var message = "this is a test message";
                await QueueTest.TestWithQueue(accountName, queueName, message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
