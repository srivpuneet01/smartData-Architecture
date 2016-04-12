using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(smartData.Startup))]
namespace smartData
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
