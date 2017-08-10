using System.IO;
using Services.Interfaces;

namespace Services.Services
{
    public class DirectoryStructureService : IDirectoryStructureService
    {
        public void CreateDirectoryStructureForSession(string rootDirectoryPath)
        {
            Directory.CreateDirectory(rootDirectoryPath);
            Directory.CreateDirectory($"{rootDirectoryPath}/A");
            Directory.CreateDirectory($"{rootDirectoryPath}/B");
            Directory.CreateDirectory($"{rootDirectoryPath}/A/{Resources.Resources.InputFolder}");
            Directory.CreateDirectory($"{rootDirectoryPath}/B/{Resources.Resources.InputFolder}");
            Directory.CreateDirectory($"{rootDirectoryPath}/A/{Resources.Resources.OutputFolder}");
            Directory.CreateDirectory($"{rootDirectoryPath}/B/{Resources.Resources.OutputFolder}");
        }

        public void DeleteDirectoryStructureForSession(string rootDirectoryPath)
        {
            Directory.Delete(rootDirectoryPath, true);
        }
    }
}
