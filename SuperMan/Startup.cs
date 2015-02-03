using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SuperMan.Startup))]
namespace SuperMan
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }
}
