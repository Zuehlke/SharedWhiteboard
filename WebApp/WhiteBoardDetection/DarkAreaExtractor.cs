using System.Collections.Generic;
using System.Drawing;
using AForge;
using WhiteBoardDetection.Interfaces;
using WhiteBoardDetection.Models;
using Image = AForge.Imaging.Image;

namespace WhiteBoardDetection
{
    public class DarkAreaExtractor : IDarkAreaExtractor
    {
        private const double MinimumSaturationOfColoredPixel = 0.21;

        public Bitmap ExtractDarkAreas(Bitmap image)
        {
            var resultImage = Image.Clone(image);

            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixel(x, y);
                    var saturation = pixel.GetSaturation();

                    if (saturation > MinimumSaturationOfColoredPixel)
                    {
                        resultImage.SetPixel(x, y, Color.OrangeRed);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return resultImage;
        }
    }
}