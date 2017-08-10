using System.Collections.Generic;
using System.Drawing;
using AForge;
using WhiteBoardDetection.Models;

namespace WhiteBoardDetection.Interfaces
{
    public interface IDarkAreaExtractor
    {
        Bitmap ExtractDarkAreas(Bitmap image);
    }
}