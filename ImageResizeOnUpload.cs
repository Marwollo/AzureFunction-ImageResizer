using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureExamples.Function
{
    public class ImageResizeOnUpload
    {
        [FunctionName("ImageResizeOnUpload")]
        public void Run([BlobTrigger("images/{name}", Connection = "imagesforresizingstorage_STORAGE")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
