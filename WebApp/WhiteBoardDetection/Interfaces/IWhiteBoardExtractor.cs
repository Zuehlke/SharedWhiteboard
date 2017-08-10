namespace WhiteBoardDetection.Interfaces
{
    public interface IWhiteBoardExtractor
    {
        void DetectAndCrop(string storageFolder, string templatesFolder);
    }
}
