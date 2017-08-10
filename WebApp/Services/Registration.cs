using Microsoft.Practices.Unity;
using Services.Interfaces;
using Services.Services;

namespace Services
{
    public static class Registration 
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ISessionService, SessionService>();
            container.RegisterType<IDirectoryStructureService, DirectoryStructureService>();
        }
    }
}
