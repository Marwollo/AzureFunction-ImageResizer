using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ImageResizer;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureExamples.Function
{
    public class ImageResizeOnUpload
    {
        [FunctionName("ImageResizeOnUpload")]
        public void Run(
            [BlobTrigger("images/{name}", Connection = "imagesforresizingstorage_STORAGE")] Stream myBlob,
            [Blob("resized-images/{name}", FileAccess.Write, Connection = "imagesforresizingstorage_STORAGE")] BlobClient output, 
            string name, 
            ILogger log)
        {
            log.LogInformation($"Received file \n Name:{name} \n with size of {myBlob.Length} Bytes");
            
            var instructions = new Instructions
            {
                Width = 320,
                Mode = FitMode.Crop,
                Scale = ScaleMode.Both
            };

            Stream stream = new MemoryStream();
            ImageBuilder.Current.Build(new ImageJob(myBlob, stream, instructions));
            stream.Seek(0, SeekOrigin.Begin);

            output.SetHttpHeaders(new BlobHttpHeaders() { ContentType = "image/png" });
            output.Upload(stream);
        }
    }
}
