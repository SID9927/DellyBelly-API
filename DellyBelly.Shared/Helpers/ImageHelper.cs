using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Shared.Helpers
{
    public static class ImageHelper
    {
        public static async Task<byte[]> ConvertToWebPAsync(Stream inputStream)
        {
            using var image = await Image.LoadAsync(inputStream);
            using var outputStream = new MemoryStream();

            var encoder = new WebpEncoder
            {
                FileFormat = WebpFileFormatType.Lossless, // lossless compression
                Quality = 75                              // adjust if you want lossy
            };

            await image.SaveAsync(outputStream, encoder);
            return outputStream.ToArray();
        }

        public static async Task<(byte[] Data, string FileName, string ContentType)> CreateWebPImageAsync(Stream inputStream, string originalFileName)
        {
            var webpBytes = await ConvertToWebPAsync(inputStream);
            var newFileName = Path.GetFileNameWithoutExtension(originalFileName) + ".webp";
            return (webpBytes, newFileName, "image/webp");
        }
    }

}
