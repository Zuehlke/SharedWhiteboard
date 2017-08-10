using AForge;

namespace WhiteBoardDetection.Models
{
    public class WhiteBoardCorners
    {
        public IntPoint? UpperLeft { get; set; }

        public IntPoint? UpperRight { get; set; }

        public IntPoint? BottomRight { get; set; }

        public IntPoint? BottomLeft { get; set; }
    }
}