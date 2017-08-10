using System;
using System.Drawing;
using AForge.Imaging.Filters;
using WhiteBoardDetection.Interfaces;

namespace WhiteBoardDetection
{
    public class SimilarityChecker : ISimilarityChecker
    {
        public double CheckSimilarity(Bitmap originalImage, Bitmap templateImage)
        {
            if (originalImage.Height > templateImage.Height || originalImage.Width > templateImage.Width)
            {
                return 0;
            }

            var resizeFilter = new ResizeBilinear(originalImage.Width, originalImage.Height);
            using (var templateImageCopy = resizeFilter.Apply(templateImage))
            {
                var height = originalImage.Height < templateImageCopy.Height
                    ? originalImage.Height
                    : templateImageCopy.Height;
                var width = originalImage.Width < templateImageCopy.Width
                    ? originalImage.Width
                    : templateImageCopy.Width;

                var count = 0;

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        var pixel1 = originalImage.GetPixel(x, y);
                        var pixel2 = templateImageCopy.GetPixel(x, y);

                        if (Math.Abs(pixel1.GetHue() - pixel2.GetHue()) <= 60 && Math.Abs(pixel1.GetSaturation() - pixel2.GetSaturation()) <= 40)
                        {
                            count++;
                        }
                    }
                }

                return (double) count / (height * width);
            }
        }
    }
}