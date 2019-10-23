using Azure.Identity;
using Azure.Storage.Queues;
using System;
using System.Threading.Tasks;

namespace AzIdentityApp
{
    internal static class QueueTest
    {
        public static async Task TestWithQueue(string accountName, string queueName, string message)
        {
            var uri = new Uri($"https://{accountName}.queue.core.windows.net/{queueName}");
            var queue = new QueueClient(uri, new DefaultAzureCredential());
            //var queue = new QueueClient(uri, new DefaultAzureCredential(new DefaultAzureCredentialOptions { IncludeEnvironmentCredential = true }));
            //var co = new DefaultAzureCredentialOptions
            //{
            //    IncludeEnvironmentCredential = true,
            //    IncludeInteractiveBrowserCredential = true,
            //    IncludeManagedIdentityCredential = true,
            //    IncludeSharedTokenCacheCredential = true
            //};
            //var queue = new QueueClient(uri, new DefaultAzureCredential(co));

            Console.WriteLine($@"calling CreateAsync() on ""{uri}""");
            await queue.CreateAsync();

            Console.WriteLine($@"calling EnqueueMessageAsync(""{message}"") on ""{uri}""");
            await queue.EnqueueMessageAsync(message);
        }
    }
}
