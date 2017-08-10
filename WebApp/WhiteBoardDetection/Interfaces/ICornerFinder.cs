using System.Collections.Generic;
using System.Drawing;
using WhiteBoardDetection.Models;

namespace WhiteBoardDetection.Interfaces
{
    public interface ICornerFinder
    {
        WhiteBoardCorners Find(Bitmap image, Bitmap template1, Bitmap template2, Bitmap template3, Bitmap template4);
    }
}