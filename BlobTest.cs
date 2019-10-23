using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AzIdentityApp
{
    internal static class BlobTest
    {
        // https://docs.microsoft.com/en-us/azure/storage/common/storage-auth-aad-msi#net-code-example-create-a-block-blob
        public async static Task CreateBlockBlobAsync(string accountName, string containerName, string blobName)
        {
            // Construct the blob container endpoint from the arguments.
            string containerEndpoint = string.Format("https://{0}.blob.core.windows.net/{1}",
                                                        accountName,
                                                        containerName);

            // Get a credential and create a client object for the blob container.
            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint),
                                                                          new DefaultAzureCredential());

            try
            {
                // Create the container if it does not exist.
                await containerClient.CreateIfNotExistsAsync();

                // Upload text to a new block blob.
                string blobContents = "This is a block blob.";
                byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);

                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    await containerClient.UploadBlobAsync(blobName, stream);
                }
            }
            catch (StorageRequestFailedException e)
            {
                Console.WriteLine(e.Message);
                //Console.ReadLine();
                throw;
            }
        }
    }
}
