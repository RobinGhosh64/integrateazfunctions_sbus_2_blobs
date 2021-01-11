using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.Azure.ServiceBus;
using System.IO;

using Azure.Storage.Blobs;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger("orders", Connection = "ServiceBusConnection",IsSessionsEnabled = true)]Microsoft.Azure.ServiceBus.Message[] orders, ILogger log)
        {


            foreach (Microsoft.Azure.ServiceBus.Message message in orders)
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {Encoding.Default.GetString(message.Body)}");

                string data = Encoding.Default.GetString(message.Body);


                //
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                //Create a unique name for the container
                string containerName = "images";

                // Create the container and return a container client object
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                string localFilePath =  "upstest" + Guid.NewGuid().ToString();

                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(localFilePath);

                Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

                // Open the file and upload its data
             
                await blobClient.UploadAsync(GenerateStreamFromString(data));
                


            }
        }

        private static System.IO.Stream GenerateStreamFromString(string s)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            StreamWriter writer = new System.IO.StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
