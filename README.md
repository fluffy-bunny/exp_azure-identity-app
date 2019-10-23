# exp\AzureIdentityApp

Simple experiment to prototype authenticating to the new `QueueClient` using
the new `Azure.Identity` SDK.

## Setup

1. Create an Azure Active Directory [application][1], e.g. `MyApp`.
2. From the portal **Overview**, make note of the `ClientId` (ApplicationId)
   and the `TenantId` (DirectoryId) -- you will need these to configure the
   console app.
3. Create a secret for the application and make note of it as well.
4. Create an Azure Storage Account, e.g. `myaccount`.
5. Create a container for blobs, e.g. `myblobs`, make note of the
   URL, e.g. `https://myaccount.blob.core.windows.net/myblobs`.
6. Click the blob in the portal to open it
   - select **Access Control (IAM)**
   - choose **Role Assignment**
   - click the `[+]` add button and select **Add role assignment** (blade
     opens on the right)
   - click in the **Role* field and enter `Storage Queue Data Contributor`
   - click in the **Select** field and enter the name of the AAD application
     created in step 1, e.g. `MyApp`
   - click the **Save** button to add enable role for the AAD application
7. Create a queue, e.g. `myqueue`, make note of the URL,
   e.g. `https://myaccount.queue.core.windows.net/myqueue`.
8. Click on the queue and assign the `Storage Queue Data Contributor` role
   to the AAD application (refer to step 6).
9. Configure the console application:
   - The values for the AAD application can go into the environment variables
     `AZURE_CLIENT_ID`, `AZURE_CLIENT_SECRET` and `AZURE_TENANT_ID`.
   - Environment variables can be prefixed with `AzIdentityApp_`.
   - Alternatively, use the `appsettings.json` or the `dotnet user-secrets`
     to specify one or more of the following:

     ```json
      {
        "ClientId": "<your Client Id from step 2",
        "TenantId": "<your Tenant Id from step 2",
        "ClientSecret": "<your secret from step 3",
        "AccountName": "myaccount",
        "ContainerName": "myblobs",
        "QueueName": "myqueue"
      }
     ```

   - You can also override any configuration from the cmdline by specify
     `<key>=<value>` arguments.
   - Any combination of the above, where the order of precedence, from lowest
     to highest is: ENVVAR, ENVVAR(`AzIdentityApp_`), `appsettings.json`,
     **user-secrets** and cmdline arguments.

## Run the experiment

``` console
dotnet restore
dotnet build
dotnet run
```

### Expected Output

``` console
calling CreateBlockBlobAsync(accountName: "myaccount", containerName: "myblobs", bloblName: "acdedc21-1a32-4aac-917f-52cb72d4858c")
calling CreateAsync() on "https://myaccount.queue.core.windows.net/myqueue"
calling EnqueueMessageAsync("this is a test message") on "https://myaccount.queue.core.windows.net/myqueue"
Success!
```

### Actual Output

``` console
calling CreateBlockBlobAsync(accountName: "myaccount", containerName: "myblobs", bloblName: "26e93e45-14c1-49d0-80dd-ccb2d9b5b463")
calling CreateAsync() on "https://myaccount.queue.core.windows.net/myqueue"
Azure.Storage.StorageRequestFailedException: This request is not authorized to perform this operation.
RequestId:<requestId>
Time:2019-10-23T07:47:42.9075015Z
Status: 403 (This request is not authorized to perform this operation.)

ErrorCode: AuthorizationFailure

Headers:
Server: Windows-Azure-Queue/1.0,Microsoft-HTTPAPI/2.0
x-ms-request-id: <requestId>
x-ms-version: 2018-11-09
x-ms-error-code: AuthorizationFailure
Date: Wed, 23 Oct 2019 07:47:42 GMT
Content-Length: 246
Content-Type: application/xml

   at Azure.Storage.Queues.QueueRestClient.Queue.CreateAsync_CreateResponse(Response response)
   at Azure.Storage.Queues.QueueRestClient.Queue.CreateAsync(HttpPipeline pipeline, Uri resourceUri, Nullable`1 timeout, IDictionary`2 metadata, String requestId, Boolean async, String operationName, CancellationToken cancellationToken)
   at Azure.Storage.Queues.QueueClient.CreateInternal(IDictionary`2 metadata, Boolean async, CancellationToken cancellationToken)
   at Azure.Storage.Queues.QueueClient.CreateAsync(IDictionary`2 metadata, CancellationToken cancellationToken)
   at AzIdentityApp.Program.TestWithQueue(String message) in S:\exp\AzIdentityApp\Program.cs:line 82
   at AzIdentityApp.Program.RunTest() in S:\exp\AzIdentityApp\Program.cs:line 63
   at AzIdentityApp.Program.Main(String[] args) in S:\exp\AzIdentityApp\Program.cs:line 36
```

[1]: https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#create-an-azure-active-directory-application