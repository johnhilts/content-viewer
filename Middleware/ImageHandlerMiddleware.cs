using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace dotnet.Middleware
{
    public class ImageHandlerMiddleware
    {
        private readonly int _maxWidth = 600;
        private readonly int _maxHeight = 400;

        // Must have constructor with this signature, otherwise exception at run time
        public ImageHandlerMiddleware(RequestDelegate next)
        {
            // This is an HTTP Handler, so no need to store next
        }

        public async Task Invoke(HttpContext context)
        {
            // var branchVer = context.Request.Query["branch"];
            // await context.Response.WriteAsync($"Branch used = {branchVer}");
            var filePath = context.Request.Path.ToString().Replace(@"/content/", @"wwwroot/content/");
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using (var image = Image.FromStream(fs))
            {
                var resizeDimensions = GetResizeDimensions(image);
                var resizedImage = Resize(image, resizeDimensions.Width, resizeDimensions.Height, RotateFlipType.RotateNoneFlipNone);
                var imageBytes = ImageToByteArray(resizedImage, "jpg", null);
                context.Response.ContentType = GetContentType();
                context.Response.ContentLength = imageBytes.Length;
                await context.Response.Body.WriteAsync(imageBytes, 0, imageBytes.Length);
            }
        }

        private (int Width, int Height) GetResizeDimensions(Image image)
        {
            var width = (int)image.Width;
            var height = (int)image.Height;
            return (Math.Min(width, _maxWidth), Math.Min(height, _maxHeight));
        }

        private string GetContentType()
        {
            return "image/jpeg";
        }

        private static Image Resize(Image image, int width, int height, RotateFlipType rotateFlipType)
        {
            // clone the Image instance, since we don't want to resize the original Image instance
            var rotatedImage = image.Clone() as Image;
            rotatedImage.RotateFlip(rotateFlipType);
            var newSize = CalculateResizedDimensions(rotatedImage, width, height);

            var resizedImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format32bppArgb);
            resizedImage.SetResolution(72, 72);

            using (var graphics = Graphics.FromImage(resizedImage))
            {
                // set parameters to create a high-quality thumbnail
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // use an image attribute in order to remove the black/gray border around image after resize
                // (most obvious on white images), see this post for more information:
                // http://www.codeproject.com/KB/GDI-plus/imgresizoutperfgdiplus.aspx
                using (var attribute = new ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);

                    // draws the resized image to the bitmap
                    graphics.DrawImage(rotatedImage, new Rectangle(new Point(0, 0), newSize), 0, 0, rotatedImage.Width, rotatedImage.Height, GraphicsUnit.Pixel, attribute);
                }
            }

            return resizedImage;
        }

        private static Size CalculateResizedDimensions(Image image, int desiredWidth, int desiredHeight)
        {
            var widthScale = (double)desiredWidth / image.Width;
            var heightScale = (double)desiredHeight / image.Height;

            // scale to whichever ratio is smaller, this works for both scaling up and scaling down
            var scale = widthScale < heightScale ? widthScale : heightScale;

            return new Size
            {
                Width = (int) (scale * image.Width),
                      Height = (int) (scale * image.Height)
            };
        }

        private static byte[] ImageToByteArray(System.Drawing.Image image, string extension, EncoderParameters encoderParameters)
        {
            var ms = new MemoryStream();
            if (!string.IsNullOrEmpty(extension) && encoderParameters != null)
                image.Save(ms, GetImageCodec(extension), encoderParameters);
            else
                image.Save(ms, image.RawFormat);
            return ms.ToArray();
        }

        private static ImageCodecInfo GetImageCodec(string extension)
        {
            extension = extension.ToUpperInvariant();
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FilenameExtension.Contains(extension))
                {
                    return codec;
                }
            }
            return codecs[1];
        }

    }

    public static class ImageHandlerExtensions
    {
        public static IApplicationBuilder UseImageHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageHandlerMiddleware>();
        }
    }
}
