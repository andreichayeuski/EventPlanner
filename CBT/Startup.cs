using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CBT.Startup))]
namespace CBT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
