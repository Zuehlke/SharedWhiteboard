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
using AForge;
using AForge.Imaging.Filters;
using Models.WhiteBoardDetection;
using WhiteBoardDetection.Interfaces;

namespace WhiteBoardDetection
{
    public class ImageRotator : IImageRotator
    {
        public Bitmap RotateImageAccordingToCorners(Bitmap image, WhiteBoardCorners whiteBoardCorners)
        {
            if (!whiteBoardCorners.UpperLeft.HasValue || !whiteBoardCorners.BottomLeft.HasValue ||
                !whiteBoardCorners.UpperRight.HasValue || !whiteBoardCorners.BottomRight.HasValue)
            {
                return image;
            }

            var angle1 = FindAngle(whiteBoardCorners.UpperLeft.Value, whiteBoardCorners.UpperRight.Value);
            var angle2 = FindAngle(whiteBoardCorners.UpperRight.Value, whiteBoardCorners.BottomRight.Value);
            var angle3 = FindAngle(whiteBoardCorners.BottomRight.Value, whiteBoardCorners.BottomLeft.Value);
            var angle4 = FindAngle(whiteBoardCorners.BottomLeft.Value, whiteBoardCorners.UpperLeft.Value);

            var angle = (angle1 + angle2 + angle3 + angle4) / 4;

            var rotationFilter = new RotateBilinear(angle);
            return rotationFilter.Apply(image);
        }

        private static double FindAngle(IntPoint point1, IntPoint point2)
        {
            var dx = point1.X - point2.X;
            var dy = point1.Y - point2.Y;

            var arctg = Math.Abs(dx) > Math.Abs(dy) ? Math.Atan((double)dy / dx) : Math.Atan((double)dx / dy);

            return arctg * 360 / (2 * Math.PI);
        }

        public Bitmap RotateImage(Bitmap image, double angle)
        {
            var rotationFilter = new RotateBilinear(angle);
            return rotationFilter.Apply(image);
        }
    }
}