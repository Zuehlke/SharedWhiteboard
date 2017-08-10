using System.Drawing;

namespace WhiteBoardDetection.Interfaces
{
    public interface ISimilarityChecker
    {
        double CheckSimilarity(Bitmap originalImage, Bitmap templateImage);
    }
}