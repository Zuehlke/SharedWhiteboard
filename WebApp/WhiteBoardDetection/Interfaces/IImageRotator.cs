using System.Drawing;
using WhiteBoardDetection.Models;

namespace WhiteBoardDetection.Interfaces
{
    public interface IImageRotator
    {
        Bitmap RotateImageAccordingToCorners(Bitmap image, WhiteBoardCorners whiteBoardCorners);

        Bitmap RotateImage(Bitmap image, double angle);
    }
}