using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace RecorteImg.Drawing.Imaging
{
    public static class ImageUtilities
    {
        private static Dictionary<string, ImageCodecInfo> encoders = null;
        private static object encodersLock = new object();
        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            get
            {
                if (encoders == null)
                {
                    lock (encodersLock)
                    {
                        if (encoders == null)
                        {
                            encoders = new Dictionary<string, ImageCodecInfo>();

                            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                            {
                                encoders.Add(codec.MimeType.ToLower(), codec);
                            }
                        }
                    }
                }

                return encoders;
            }
        }

        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            return result;
        }

        public static void SaveJpeg(string path, Image image, int quality)
        {
            if ((quality < 0) || (quality > 100))
            {
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                throw new ArgumentOutOfRangeException(error);
            }

            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            image.Save(path, jpegCodec, encoderParams);
        }

        public static void SavePng(string path, Image image, int quality)
        {
            if ((quality < 0) || (quality > 100))
            {
                string error = string.Format("Png image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                throw new ArgumentOutOfRangeException(error);
            }

            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/png");

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            image.Save(path, jpegCodec, encoderParams);
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            string lookupKey = mimeType.ToLower();

            ImageCodecInfo foundCodec = null;

            if (Encoders.ContainsKey(lookupKey))
            {
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }
    }
}