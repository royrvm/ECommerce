using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ECommerce.Backend.Startup))]
namespace ECommerce.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
