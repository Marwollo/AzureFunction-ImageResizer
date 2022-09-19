using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

namespace AzureExamples.Function
{
    public class ImageResizeOnUpload
    {
        [FunctionName("ImageResizeOnUpload")]
        public void Run(
            [BlobTrigger("images/{name}", Connection = "imagesforresizingstorage_STORAGE")] Stream input,
            [Blob("resized-images/resized-{name}", FileAccess.Write, Connection = "imagesforresizingstorage_STORAGE")] Stream output,
            string name, 
            ILogger log)
        {
            log.LogInformation($"Received file with name: {name}, with size of {input.Length} Bytes");
            using (Image image = Image.Load(input)) {
                image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
                image.Save(output, new PngEncoder());
            }
            log.LogInformation($"Successfully finished resizing the file with name: {name}");
        }
    }
}
