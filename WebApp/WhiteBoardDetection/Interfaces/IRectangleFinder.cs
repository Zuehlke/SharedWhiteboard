using System.Collections.Generic;
using System.Drawing;
using Models.WhiteBoardDetection;
using WhiteBoardDetection.Models;

namespace WhiteBoardDetection.Interfaces
{
    public interface IRectangleFinder
    {
        IReadOnlyCollection<RectangularContour> Find(Bitmap image);
    }
}
