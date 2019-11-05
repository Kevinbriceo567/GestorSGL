using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SGLClientBase.Startup))]
namespace SGLClientBase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
