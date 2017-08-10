using System;
using System.Drawing;
using WhiteBoardDetection.Models;

namespace Models.WhiteBoardDetection
{
    public class WhiteBoardRectangle
    {
        public int X { get; }

        public int Y { get; }

        public int Width { get; }

        public int Height { get; }

        public WhiteBoardRectangle(Bitmap image, WhiteBoardCorners corners)
        {
            if (!corners.UpperLeft.HasValue && !corners.UpperRight.HasValue && !corners.BottomLeft.HasValue &&
                !corners.BottomRight.HasValue)
            {
                throw new ArgumentException("At least one point has to be set.");
            }

            var startingX = 0;
            var startingY = 0;
            var finalX = image.Width - 1;
            var finalY = image.Height - 1;

            if (corners.UpperLeft.HasValue || corners.BottomLeft.HasValue)
            {
                if (corners.UpperLeft.HasValue && corners.BottomLeft.HasValue)
                {
                    startingX = corners.UpperLeft.Value.X < corners.BottomLeft.Value.X
                        ? corners.UpperLeft.Value.X
                        : corners.BottomLeft.Value.X;
                }
                else
                {
                    startingX = corners.UpperLeft.HasValue 
                        ? corners.UpperLeft.Value.X 
                        : corners.BottomLeft.Value.X;
                }
            }

            if (corners.UpperLeft.HasValue || corners.UpperRight.HasValue)
            {
                if (corners.UpperLeft.HasValue && corners.UpperRight.HasValue)
                {
                    startingY = corners.UpperLeft.Value.Y < corners.UpperRight.Value.Y
                        ? corners.UpperLeft.Value.Y
                        : corners.UpperRight.Value.Y;
                }
                else
                {
                    startingY = corners.UpperLeft.HasValue 
                        ? corners.UpperLeft.Value.Y 
                        : corners.UpperRight.Value.Y;
                }
            }

            if (corners.UpperRight.HasValue || corners.BottomRight.HasValue)
            {
                if (corners.UpperRight.HasValue && corners.BottomRight.HasValue)
                {
                    finalX = corners.UpperRight.Value.X > corners.BottomRight.Value.X
                        ? corners.UpperRight.Value.X
                        : corners.BottomRight.Value.X;
                }
                else
                {
                    finalX = corners.UpperRight.HasValue 
                        ? corners.UpperRight.Value.X 
                        : corners.BottomRight.Value.X;
                }
            }

            if (corners.BottomLeft.HasValue || corners.BottomRight.HasValue)
            {
                if (corners.BottomLeft.HasValue && corners.BottomRight.HasValue)
                {
                    finalY = corners.BottomLeft.Value.Y > corners.BottomRight.Value.Y
                        ? corners.BottomLeft.Value.Y
                        : corners.BottomRight.Value.Y;
                }
                else
                {
                    finalY = corners.BottomLeft.HasValue 
                        ? corners.BottomLeft.Value.Y 
                        : corners.BottomRight.Value.Y;
                }
            }

            X = startingX;
            Y = startingY;
            Width = finalX - startingX;
            Height = finalY - startingY;
        }

        public WhiteBoardRectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}