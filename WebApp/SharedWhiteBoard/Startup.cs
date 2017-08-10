using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Owin;
using Services;

[assembly: OwinStartup(typeof(SharedWhiteBoard.Startup))]

namespace SharedWhiteBoard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
