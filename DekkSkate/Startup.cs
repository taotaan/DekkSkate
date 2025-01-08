using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DekkSkate.Startup))]
namespace DekkSkate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
