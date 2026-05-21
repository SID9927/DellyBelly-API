using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DellyBelly.Shared.Helpers
{
    public static class ImageHelper
    {
        // Max dimension (width or height) before we downscale.
        // 1920px is Full-HD – more than enough for any product/category image.
        private const int MaxDimension = 1920;

        // Lossy WebP quality (0-100). 85 is the industry sweet-spot:
        // visually near-lossless but typically 3-5× smaller than the original JPEG/PNG.
        private const int WebPQuality = 85;

        /// <summary>
        /// Converts any image stream to a lossy WebP byte array.
        /// Images wider/taller than <see cref="MaxDimension"/> are downscaled
        /// proportionally before encoding so they are never stored oversized.
        /// </summary>
        public static async Task<byte[]> ConvertToWebPAsync(Stream inputStream)
        {
            using var image = await Image.LoadAsync(inputStream);

            // Downscale only if the image exceeds the max dimension
            if (image.Width > MaxDimension || image.Height > MaxDimension)
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(MaxDimension, MaxDimension),
                    Sampler = KnownResamplers.Lanczos3  // high-quality downscale kernel
                }));
            }

            using var outputStream = new MemoryStream();

            // LOSSY WebP – Quality IS honoured here (unlike Lossless mode).
            // Lossless WebP ignores Quality and stores raw pixel data, which
            // inflates small JPEGs by 4-5× – that was the original bug.
            var encoder = new WebpEncoder
            {
                FileFormat = WebpFileFormatType.Lossy,
                Quality = WebPQuality
            };

            await image.SaveAsync(outputStream, encoder);
            return outputStream.ToArray();
        }

        /// <summary>
        /// Converts an image stream to WebP and returns the bytes, new filename,
        /// and MIME type ready to be stored in the database.
        /// </summary>
        public static async Task<(byte[] Data, string FileName, string ContentType)> CreateWebPImageAsync(
            Stream inputStream, string originalFileName)
        {
            var webpBytes = await ConvertToWebPAsync(inputStream);
            var newFileName = Path.GetFileNameWithoutExtension(originalFileName) + ".webp";
            return (webpBytes, newFileName, "image/webp");
        }
    }
}
