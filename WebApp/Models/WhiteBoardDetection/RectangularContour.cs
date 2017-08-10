using System.Collections.Generic;
using System.Linq;
using AForge;
using WhiteBoardDetection.Models;

namespace Models.WhiteBoardDetection
{
    public class RectangularContour
    {
        public RectangularContour(IReadOnlyCollection<IntPoint> contour)
        {
            Contour = contour;

            CalculateProperties();
        }

        private void CalculateProperties()
        {
            var pointsWithSmallerX = Contour.OrderBy(c => c.X).Take(2).ToArray();

            if (pointsWithSmallerX[0].Y < pointsWithSmallerX[1].Y)
            {
                UpperLeft = pointsWithSmallerX[0];
                BottomLeft = pointsWithSmallerX[1];
            }
            else
            {
                UpperLeft = pointsWithSmallerX[1];
                BottomLeft = pointsWithSmallerX[0];
            }

            var pointsWithLargerX = Contour.OrderByDescending(c => c.X).Take(2).ToArray();

            if (pointsWithLargerX[0].Y < pointsWithLargerX[1].Y)
            {
                UpperRight = pointsWithLargerX[0];
                BottomRight = pointsWithLargerX[1];
            }
            else
            {
                UpperRight = pointsWithLargerX[1];
                BottomRight = pointsWithLargerX[0];
            }
        }

        public RectangularContour(WhiteBoardRectangle whiteBoardRectangle)
        {
            Contour = new []
            {
                new IntPoint(whiteBoardRectangle.X, whiteBoardRectangle.Y + whiteBoardRectangle.Height),
                new IntPoint(whiteBoardRectangle.X + whiteBoardRectangle.Width, whiteBoardRectangle.Y + whiteBoardRectangle.Height),
                new IntPoint(whiteBoardRectangle.X + whiteBoardRectangle.Width, whiteBoardRectangle.Y),
                new IntPoint(whiteBoardRectangle.X, whiteBoardRectangle.Y),   
            };

            CalculateProperties();
        }

        public IReadOnlyCollection<IntPoint> Contour { get; }

        public IntPoint UpperLeft { get; private set; }

        public IntPoint UpperRight { get; private set; }

        public IntPoint BottomRight { get; private set; }

        public IntPoint BottomLeft { get; private set; }

        public IntPoint this[int index] => Contour.ElementAt(index);

        public long Size => Contour.Count;
    }
}