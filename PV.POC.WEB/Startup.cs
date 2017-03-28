using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PV.POC.WEB.Startup))]
namespace PV.POC.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
