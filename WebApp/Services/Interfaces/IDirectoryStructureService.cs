namespace Services.Interfaces
{
    public interface IDirectoryStructureService
    {
        void CreateDirectoryStructureForSession(string rootDirectoryPath);

        void DeleteDirectoryStructureForSession(string rootDirectoryPath);
    }
}
