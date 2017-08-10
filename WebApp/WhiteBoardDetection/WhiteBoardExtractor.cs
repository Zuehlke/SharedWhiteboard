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

using System.Diagnostics;
using System.Drawing;
using AForge.Imaging.Filters;
using Models.WhiteBoardDetection;
using WhiteBoardDetection.Interfaces;
using Image = AForge.Imaging.Image;


namespace WhiteBoardDetection
{
    public class WhiteBoardExtractor : IWhiteBoardExtractor
    {
        private const string InputImagePath = "\\input\\image.jpg";
        private const string Template1ImagePath = "\\template1.jpg";
        private const string Template2ImagePath = "\\template2.jpg";
        private const string Template3ImagePath = "\\template3.jpg";
        private const string Template4ImagePath = "\\template4.jpg";
        private const string OutputImagePath = "\\output\\image.jpg";
        private const string DarkOutputImagePath = "\\output\\dark.jpg";

        private readonly ICornerFinder _cornerFinder;
        private readonly IImageRotator _imageRotator;
        private readonly DarkAreaExtractor _darkAreaExtractor;

        public WhiteBoardExtractor(ICornerFinder cornerFinder, IImageRotator imageRotator, DarkAreaExtractor darkAreaExtractor)
        {
            _cornerFinder = cornerFinder;
            _imageRotator = imageRotator;
            _darkAreaExtractor = darkAreaExtractor;
        }
        
        public void DetectAndCrop(string storageFolder, string templatesFolder)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var image = Image.FromFile($"{storageFolder}{InputImagePath}");

            var template1 = Image.FromFile($"{templatesFolder}{Template1ImagePath}");
            var template2 = Image.FromFile($"{templatesFolder}{Template2ImagePath}");
            var template3 = Image.FromFile($"{templatesFolder}{Template3ImagePath}");
            var template4 = Image.FromFile($"{templatesFolder}{Template4ImagePath}");

            var corners = _cornerFinder.Find(image, template1, template2, template3, template4);
            var whiteBoardRectangle = new WhiteBoardRectangle(image, corners);

            // TODO Right now, this does nothing. Fix it.
            //image = _imageRotator.RotateImageAccordingToCorners(image, corners);

            var cropFilter = new Crop(new Rectangle(whiteBoardRectangle.X, whiteBoardRectangle.Y, whiteBoardRectangle.Width, whiteBoardRectangle.Height));
            image = cropFilter.Apply(image);

            // HoloLens needs image that is upside-down
            image = _imageRotator.RotateImage(image, 180);

            image.Save($"{storageFolder}{OutputImagePath}");

            var darkImage = _darkAreaExtractor.ExtractDarkAreas(image);
            darkImage.Save($"{storageFolder}{DarkOutputImagePath}");
            
            stopwatch.Stop();

            Debug.WriteLine($"Whiteboard extraction took {stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}
