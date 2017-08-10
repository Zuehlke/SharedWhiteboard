using System.Drawing;
using AForge.Imaging.Filters;
using WhiteBoardDetection.Interfaces;
using WhiteBoardDetection.Models;
using Image = AForge.Imaging.Image;

namespace WhiteBoardDetection
{
    public class CornerFinderAccordingToRectangles : ICornerFinder
    {
        private readonly ISimilarityChecker _similarityChecker;
        private readonly IImageRotator _imageRotator;
        private readonly IRectangleFinder _rectangleFinder;

        public CornerFinderAccordingToRectangles(ISimilarityChecker similarityChecker, IImageRotator imageRotator, IRectangleFinder rectangleFinder)
        {
            _similarityChecker = similarityChecker;
            _imageRotator = imageRotator;
            _rectangleFinder = rectangleFinder;
        }

        public WhiteBoardCorners Find(Bitmap image, Bitmap template1, Bitmap template2, Bitmap template3, Bitmap template4)
        {
            var grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            var imageCopy = grayscaleFilter.Apply(image);
            var rectangles = _rectangleFinder.Find(imageCopy);

            var maxSimilarity1 = 0d;
            var maxSimilarity2 = 0d;
            var maxSimilarity3 = 0d;
            var maxSimilarity4 = 0d;
            
            var whiteBoardCorners = new WhiteBoardCorners();

            foreach (var rectangle in rectangles)
            {
                var temporaryImage = Image.Clone(image);
                //temporaryImage = _imageRotator.RotateImageAccordingToRectangularContour(temporaryImage, rectangle);

                var startingX = rectangle.UpperLeft.X < rectangle.BottomLeft.X ? rectangle.UpperLeft.X : rectangle.BottomLeft.X;
                var startingY = rectangle.UpperLeft.Y < rectangle.UpperRight.Y ? rectangle.UpperLeft.Y : rectangle.UpperRight.Y;
                var finalX = rectangle.UpperRight.X > rectangle.BottomRight.X ? rectangle.UpperRight.X : rectangle.BottomRight.X;
                var finalY = rectangle.BottomLeft.Y > rectangle.BottomRight.Y ? rectangle.BottomLeft.Y : rectangle.BottomRight.Y;

                var cropFilter = new Crop(new Rectangle(startingX, startingY, finalX - startingX, finalY - startingY));
                temporaryImage = cropFilter.Apply(temporaryImage);

                var resizeFilter = new ResizeBilinear(temporaryImage.Width, temporaryImage.Height);

                var temporaryTemplate = Image.Clone(template1);
                temporaryTemplate = resizeFilter.Apply(temporaryTemplate);

                var similarity1 = _similarityChecker.CheckSimilarity(temporaryImage, temporaryTemplate);
                if (similarity1 > 0.5 && similarity1 > maxSimilarity1)
                {
                    maxSimilarity1 = similarity1;
                    whiteBoardCorners.UpperLeft = rectangle.UpperLeft;
                }

                temporaryTemplate = Image.Clone(template2);
                temporaryTemplate = resizeFilter.Apply(temporaryTemplate);

                var similarity2 = _similarityChecker.CheckSimilarity(temporaryImage, temporaryTemplate);
                if (similarity2 > 0.5 && similarity2 > maxSimilarity2)
                {
                    maxSimilarity2 = similarity2;
                    whiteBoardCorners.UpperRight = rectangle.UpperRight;
                }

                temporaryTemplate = Image.Clone(template3);
                temporaryTemplate = resizeFilter.Apply(temporaryTemplate);

                var similarity3 = _similarityChecker.CheckSimilarity(temporaryImage, temporaryTemplate);
                if (similarity3 > 0.5 && similarity3 > maxSimilarity3)
                {
                    maxSimilarity3 = similarity3;
                    whiteBoardCorners.BottomLeft = rectangle.BottomLeft;
                }

                temporaryTemplate = Image.Clone(template4);
                temporaryTemplate = resizeFilter.Apply(temporaryTemplate);

                var similarity4 = _similarityChecker.CheckSimilarity(temporaryImage, temporaryTemplate);
                if (similarity4 > 0.5 && similarity4 > maxSimilarity4)
                {
                    maxSimilarity4 = similarity4;
                    whiteBoardCorners.BottomRight = rectangle.BottomRight;
                }
            }

            return whiteBoardCorners;
        }
    }
}