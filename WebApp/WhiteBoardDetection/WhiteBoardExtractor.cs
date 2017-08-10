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
