using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Darkly.Startup))]
namespace Darkly
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
