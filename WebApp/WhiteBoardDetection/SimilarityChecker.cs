//
// Copyright 2017, Zühlke
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// 
// 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer 
//    in the documentation and/or other materials  provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF  MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS 
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL  DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER  IN CONTRACT, STRICT 
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

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