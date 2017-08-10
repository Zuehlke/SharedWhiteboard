using System;
using System.Collections.Generic;
using System.Drawing;
using AForge;
using AForge.Imaging.Filters;
using Models.WhiteBoardDetection;
using WhiteBoardDetection.Interfaces;
using WhiteBoardDetection.Models;

namespace WhiteBoardDetection
{
    public class CornerFinderAccordingToColors : ICornerFinder
    {
        private const double PurpleColorHueMinimumValue = 233;
        private const double PurpleColorHueMaximumValue = 300;

        private const double BlueColorHueMinimumValue = 172;
        private const double BlueColorHueMaximumValue = 230;

        private const double RedColorHueMinimumValue = 345;
        private const double RedColorHueMaximumValue = 15;

        private const double GreenColorHueMinimumValue = 75;
        private const double GreenColorHueMaximumValue = 157;

        private const double MinimumSaturation = 0.08;

        private readonly ISimilarityChecker _similarityChecker;

        public CornerFinderAccordingToColors(ISimilarityChecker similarityChecker)
        {
            _similarityChecker = similarityChecker;
        }
        
        public WhiteBoardCorners Find(Bitmap image, Bitmap template1, Bitmap template2, Bitmap template3, Bitmap template4)
        {
            var whiteBoardCorners = new WhiteBoardCorners();
            
            var template1StartingPointCandidates = new List<IntPoint>();
            var template1FinalPointCandidates = new List<IntPoint>();

            var template2StartingPointCandidates = new List<IntPoint>();
            var template2FinalPointCandidates = new List<IntPoint>();

            var template3StartingPointCandidates = new List<IntPoint>();
            var template3FinalPointCandidates = new List<IntPoint>();

            var template4StartingPointCandidates = new List<IntPoint>();
            var template4FinalPointCandidates = new List<IntPoint>();

            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    CheckIfPixelIsTemplateStartingPointOrFinalPointCandidate(
                        image, 
                        x, 
                        y, 
                        template1StartingPointCandidates, 
                        template1FinalPointCandidates, 
                        IsPurple);

                    CheckIfPixelIsTemplateStartingPointOrFinalPointCandidate(
                        image,
                        x,
                        y,
                        template2StartingPointCandidates,
                        template2FinalPointCandidates,
                        IsBlue);

                    CheckIfPixelIsTemplateStartingPointOrFinalPointCandidate(
                        image,
                        x,
                        y,
                        template3StartingPointCandidates,
                        template3FinalPointCandidates,
                        IsRed);

                    CheckIfPixelIsTemplateStartingPointOrFinalPointCandidate(
                        image,
                        x,
                        y,
                        template4StartingPointCandidates,
                        template4FinalPointCandidates,
                        IsGreen);
                }
            }

            var rectangle1 = FindMaximumMatch(image, template1, template1StartingPointCandidates, template1FinalPointCandidates);
            if (rectangle1 != null)
            {
                whiteBoardCorners.UpperLeft = new IntPoint(rectangle1.X, rectangle1.Y);
            }
            var rectangle2 = FindMaximumMatch(image, template2, template2StartingPointCandidates, template2FinalPointCandidates);
            if (rectangle2 != null)
            {
                whiteBoardCorners.UpperRight = new IntPoint(rectangle2.X + rectangle2.Width, rectangle2.Y);
            }

            var rectangle3 = FindMaximumMatch(image, template3, template3StartingPointCandidates, template3FinalPointCandidates);
            if (rectangle3 != null)
            {
                whiteBoardCorners.BottomLeft = new IntPoint(rectangle3.X, rectangle3.Y + rectangle3.Height);
            }

            var rectangle4 = FindMaximumMatch(image, template4, template4StartingPointCandidates, template4FinalPointCandidates);
            if (rectangle4 != null)
            {
                whiteBoardCorners.BottomRight = new IntPoint(rectangle4.X + rectangle4.Width, rectangle4.Y + rectangle4.Height);
            }

            return whiteBoardCorners;;
        }

        private WhiteBoardRectangle FindMaximumMatch(
            Bitmap image, 
            Bitmap template, 
            IEnumerable<IntPoint> templateStartingPointCandidates,
            IReadOnlyCollection<IntPoint> templateFinalPointCandidates)
        {
            double maximumTemplate1Surface = 0;
            IntPoint? maximumMatchStartingPoint = null;
            IntPoint? maximumMatchFinalPoint = null;

            foreach (var templateStartingPointCandidate in templateStartingPointCandidates)
            {
                foreach (var templateFinalPointCandidate in templateFinalPointCandidates)
                {
                    var width = templateFinalPointCandidate.X - templateStartingPointCandidate.X;
                    var height = templateFinalPointCandidate.Y - templateStartingPointCandidate.Y;
                    var surface = width * height;

                    if (height <= 0 || width <= 0 || surface < 700)
                    {
                        continue;
                    }

                    var cropFilter = new Crop(new Rectangle(templateStartingPointCandidate.X, templateStartingPointCandidate.Y, width, height));

                    using (var croppedImage = cropFilter.Apply(image))
                    {
                        var similarity = _similarityChecker.CheckSimilarity(croppedImage, template);

                        if (similarity > 0.5 && surface > maximumTemplate1Surface)
                        {
                            maximumMatchStartingPoint = templateStartingPointCandidate;
                            maximumMatchFinalPoint = templateFinalPointCandidate;
                            maximumTemplate1Surface = surface;
                        }
                    }
                }
            }

            if (maximumMatchStartingPoint.HasValue && maximumMatchFinalPoint.HasValue)
            {
                var width = maximumMatchFinalPoint.Value.X - maximumMatchStartingPoint.Value.X;
                var height = maximumMatchFinalPoint.Value.Y - maximumMatchStartingPoint.Value.Y;

                return new WhiteBoardRectangle(maximumMatchStartingPoint.Value.X, maximumMatchStartingPoint.Value.Y, width, height);
            }

            return null;
        }

        private static void CheckIfPixelIsTemplateStartingPointOrFinalPointCandidate(
            Bitmap image, 
            int x, 
            int y,
            ICollection<IntPoint> template1StartingPointCandidates, 
            ICollection<IntPoint> template1FinalPointCandidates, 
            Func<Color, bool> checkIfColorMatches)
        {
            var pixel = image.GetPixel(x, y);

            if (checkIfColorMatches(pixel))
            {
                var leftIsMatched = false;
                if (x - 1 >= 0)
                {
                    var leftPixel = image.GetPixel(x - 1, y);
                    leftIsMatched = checkIfColorMatches(leftPixel);
                }

                var upperIsMatched = false;
                if (y - 1 >= 0)
                {
                    var upperPixel = image.GetPixel(x, y - 1);
                    upperIsMatched = checkIfColorMatches(upperPixel);
                }

                if (!leftIsMatched && !upperIsMatched)
                {
                    template1StartingPointCandidates.Add(new IntPoint(x, y));
                }

                var rightIsMatched = false;
                if (x + 1 < image.Width)
                {
                    var rightPixel = image.GetPixel(x + 1, y);
                    rightIsMatched = checkIfColorMatches(rightPixel);
                }

                var bottomIsMatched = false;
                if (y + 1 < image.Height)
                {
                    var bottomPixel = image.GetPixel(x, y + 1);
                    bottomIsMatched = checkIfColorMatches(bottomPixel);
                }

                if (!rightIsMatched && !bottomIsMatched)
                {
                    template1FinalPointCandidates.Add(new IntPoint(x, y));
                }
            }
        }

        private static bool IsPurple(Color color)
        {
            var hue = color.GetHue();
            return hue >= PurpleColorHueMinimumValue && hue <= PurpleColorHueMaximumValue && color.GetSaturation() >= MinimumSaturation;
        }

        private static bool IsBlue(Color color)
        {
            var hue = color.GetHue();
            return hue >= BlueColorHueMinimumValue && hue <= BlueColorHueMaximumValue && color.GetSaturation() >= MinimumSaturation;
        }

        private static bool IsRed(Color color)
        {
            var hue = color.GetHue();
            return (hue >= 0 && hue <= RedColorHueMaximumValue ||
                   hue >= RedColorHueMinimumValue && hue < 360) && 
                   color.GetSaturation() >= MinimumSaturation;
        }

        private static bool IsGreen(Color color)
        {
            var hue = color.GetHue();
            return hue >= GreenColorHueMinimumValue && hue <= GreenColorHueMaximumValue && color.GetSaturation() >= MinimumSaturation;
        }
    }
}
