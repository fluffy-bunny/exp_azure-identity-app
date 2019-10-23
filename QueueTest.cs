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

            Console.WriteLine($@"calling CreateAsync() on ""{uri}""");
            await queue.CreateAsync();

            Console.WriteLine($@"calling EnqueueMessageAsync(""{message}"") on ""{uri}""");
            await queue.EnqueueMessageAsync(message);
        }
    }
}
