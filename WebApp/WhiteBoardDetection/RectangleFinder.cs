using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using Models.WhiteBoardDetection;
using WhiteBoardDetection.Interfaces;
using Point = System.Drawing.Point;

namespace WhiteBoardDetection
{
    public class RectangleFinder : IRectangleFinder
    {
        private const float AngleDeviationTolerance = 5f;

        private const float ContraryDistanceDeviationTolerance = 5f;

        private const float MinimumRectangleSurface = 800f;
        private const int DistanceBetweenCenterDeviationTolerance = 1;
        private const int PointSimilarityTolerance = 5;

        public IReadOnlyCollection<RectangularContour> Find(Bitmap image)
        {
            var medianFilter = new Median(3);
            image = medianFilter.Apply(image);

            var cannyFilter = new CannyEdgeDetector(1, 10);
            cannyFilter.ApplyInPlace(image);

            var blobCounter = new BlobCounter();
            blobCounter.ProcessImage(image);
            var blobs = blobCounter.GetObjectsInformation();
            
            var rectangles = new List<RectangularContour>();


            foreach (var blob in blobs)
            {
                var edgePoints = blobCounter.GetBlobsEdgePoints(blob);

                if (edgePoints.Count <= 4)
                {
                    continue;
                }

                var corners = PointsCloud.FindQuadrilateralCorners(edgePoints);

                if (IsQuadrilateral(corners, edgePoints))
                {
                    if (IsQuadrilateral(corners, edgePoints) && IsRectangle(corners) &&
                        !IsSimilarRectangleFound(rectangles, corners))
                    {
                        rectangles.Add(new RectangularContour(corners));
                    }
                }
            }

            return rectangles;
        }

        private static bool IsSimilarRectangleFound(IReadOnlyCollection<RectangularContour> rectangles, IReadOnlyCollection<IntPoint> corners)
        {
            return rectangles.Any(rectangle => ContoursAreSimilar(rectangle.Contour, corners));
        }

        private static bool ContoursAreSimilar(IReadOnlyCollection<IntPoint> contour1, IReadOnlyCollection<IntPoint> contour2)
        {
            foreach (var point1 in contour1)
            {
                foreach (var point2 in contour2)
                {
                    if (Math.Abs(point1.X - point2.X) > PointSimilarityTolerance || Math.Abs(point1.Y - point2.Y) > PointSimilarityTolerance)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool IsQuadrilateral(IReadOnlyList<IntPoint> corners, IReadOnlyCollection<IntPoint> edgePoints)
        {
            if (corners.Count != 4)
            {
                return false;
            }

            var sides = new Line[corners.Count];

            for (var i = 0; i < corners.Count; i++)
            {
                var currentPoint = corners[i];
                var nextPoint = i == corners.Count - 1 ? corners[0] : corners[i + 1];

                sides[i] = Line.FromPoints(currentPoint, nextPoint);
            }

            var meanDistance = edgePoints.Sum(edgePoint => sides.Min(s => s.DistanceToPoint(edgePoint))) / edgePoints.Count;

            // ReSharper disable once InconsistentNaming
            IntPoint minXY;

            // ReSharper disable once InconsistentNaming
            IntPoint maxXY;
            PointsCloud.GetBoundingRectangle(corners, out minXY, out maxXY);

            var rectangleSize = maxXY - minXY;

            var maxDistance = Math.Max(0.5f, (float) (rectangleSize.X + rectangleSize.Y) / 2 * 0.03f);

            return meanDistance <= maxDistance;
        }

        private static bool IsRectangle(IReadOnlyCollection<IntPoint> corners)
        {
            if (corners.Count != 4)
            {
                return false;
            }

            var point1 = corners.ElementAt(0);
            var point2 = corners.ElementAt(1);
            var point3 = corners.ElementAt(2);
            var point4 = corners.ElementAt(3);

            // Check if distances between center and all 4 corners are equal 
            var cx = (float)(point1.X + point2.X + point3.X + point4.X) / corners.Count;
            var cy = (float)(point1.Y + point2.Y + point3.Y + point4.Y) / corners.Count;

            var dd1 = Math.Pow(cx - point1.X, 2) + Math.Pow(cy - point1.Y, 2);
            var dd2 = Math.Pow(cx - point1.X, 2) + Math.Pow(cy - point1.Y, 2);
            var dd3 = Math.Pow(cx - point1.X, 2) + Math.Pow(cy - point1.Y, 2);
            var dd4 = Math.Pow(cx - point1.X, 2) + Math.Pow(cy - point1.Y, 2);

            if (!(Math.Abs(dd1 - dd2) < DistanceBetweenCenterDeviationTolerance && Math.Abs(dd1 - dd3) < DistanceBetweenCenterDeviationTolerance && Math.Abs(dd1 - dd4) < DistanceBetweenCenterDeviationTolerance))
            {
                return false;
            };

            // Check if all angles are close to 90 degrees
            var side1 = Line.FromPoints(point1, point2);
            var side2 = Line.FromPoints(point2, point3);
            var side3 = Line.FromPoints(point3, point4);
            var side4 = Line.FromPoints(point4, point1);

            if (Math.Abs(90 - side1.GetAngleBetweenLines(side2)) > AngleDeviationTolerance)
            {
                return false;
            }
            if (Math.Abs(90 - side2.GetAngleBetweenLines(side3)) > AngleDeviationTolerance)
            {
                return false;
            }
            if (Math.Abs(90 - side3.GetAngleBetweenLines(side4)) > AngleDeviationTolerance)
            {
                return false;
            }
            if (Math.Abs(90 - side4.GetAngleBetweenLines(side1)) > AngleDeviationTolerance)
            {
                return false;
            }

            // Check if contrary sides are of the same length
            var distance1 = point1.DistanceTo(point2);
            var distance2 = point2.DistanceTo(point3);

            if (Math.Abs(distance1 - point3.DistanceTo(point4)) > ContraryDistanceDeviationTolerance)
            {
                return false;
            }
            if (Math.Abs(distance2 - point1.DistanceTo(point4)) > ContraryDistanceDeviationTolerance)
            {
                return false;
            }

            // Filter out small rectangles because they can be considered as noise
            return distance1 * distance2 >= MinimumRectangleSurface;
        }

        private static Point[] ToPointsArray(IEnumerable<IntPoint> corners)
        {
            return corners.Select(corner => new Point(corner.X, corner.Y)).ToArray();
        }
    }

}
