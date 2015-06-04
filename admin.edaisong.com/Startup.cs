using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(tools.edaisong.com.Startup))]
namespace tools.edaisong.com
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
