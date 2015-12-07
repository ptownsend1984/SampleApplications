using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GMailLabelCleanup.Startup))]
namespace GMailLabelCleanup
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
