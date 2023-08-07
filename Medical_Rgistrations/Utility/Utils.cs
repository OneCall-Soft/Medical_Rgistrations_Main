using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Compression;

namespace DBAccess.Utility
{
    public static class Utils
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void Compressimage(Stream srcImgStream, string targetPath)
        {
            try
            {
                // Convert stream to image
                using var image = System.Drawing.Image.FromStream(srcImgStream);

                float maxHeight = 900.0f;
                float maxWidth = 900.0f;
                int newWidth;
                int newHeight;

                var originalBMP = new Bitmap(srcImgStream);
                int originalWidth = originalBMP.Width;
                int originalHeight = originalBMP.Height;

                if (originalWidth > maxWidth || originalHeight > maxHeight)
                {
                    // To preserve the aspect ratio  
                    float ratioX = (float)maxWidth / (float)originalWidth;
                    float ratioY = (float)maxHeight / (float)originalHeight;
                    float ratio = Math.Min(ratioX, ratioY);
                    newWidth = (int)(originalWidth * ratio);
                    newHeight = (int)(originalHeight * ratio);
                }

                else
                {
                    newWidth = (int)originalWidth;
                    newHeight = (int)originalHeight;
                }

                var bitmap = new Bitmap(originalBMP, newWidth, newHeight);
                var imgGraph = Graphics.FromImage(bitmap);

                imgGraph.SmoothingMode = SmoothingMode.Default;
                imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

                var extension = Path.GetExtension(targetPath).ToLower();
                // for file extension having png and gif
                if (extension == ".png" || extension == ".gif")
                {
                    // Save image to targetPath
                    bitmap.Save(targetPath, image.RawFormat);
                }

                // for file extension having .jpg or .jpeg
                else if (extension == ".jpg" || extension == ".jpeg")
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    var parameter = new EncoderParameter(myEncoder, 50);
                    encoderParameters.Param[0] = parameter;

                    // Save image to targetPath
                    bitmap.Save(targetPath, jpgEncoder, encoderParameters);
                }
                bitmap.Dispose();
                imgGraph.Dispose();
                originalBMP.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        public static void CompressFile(string path)
        {
            FileStream sourceFile = File.OpenRead(path);
            FileStream destinationFile = File.Create(path + ".gz");

            byte[] buffer = new byte[sourceFile.Length];
            sourceFile.Read(buffer, 0, buffer.Length);

            using (GZipStream output = new GZipStream(destinationFile,
                CompressionMode.Compress))
            {
                //Console.WriteLine("Compressing {0} to {1}.", sourceFile.Name,
                //    destinationFile.Name, false);

                output.Write(buffer, 0, buffer.Length);
            }

            // Close the files.
            sourceFile.Close();
            destinationFile.Close();
        }
      
    }
}
