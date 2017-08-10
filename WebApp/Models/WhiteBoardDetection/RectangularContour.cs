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

using System.Collections.Generic;
using System.Linq;
using AForge;

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