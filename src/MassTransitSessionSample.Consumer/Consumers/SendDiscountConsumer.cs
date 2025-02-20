using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MassTransit;
using Newtonsoft.Json;
using System.Text;

namespace MassTransitSessionSample
{
    public class SendDiscountConsumer : IConsumer<SendDiscount>
    {
        private readonly BlobServiceClient _blobServiceClient;
        public SendDiscountConsumer(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task Consume(ConsumeContext<SendDiscount> context)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("masstransit-session-sample");
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            BlobClient blobClient = containerClient.GetBlobClient($"{context.Message.Creation.ToString("yyyy-MM-dd-HH-mm-ss-fff")}__{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff")}.json");

            JsonConvert.SerializeObject(context.Message);
            var fileByteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(context.Message));

            using (MemoryStream stream = new(fileByteArray))
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "application/json" });
            }

            Thread.Sleep(5000);
        }
    }
}
