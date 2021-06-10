using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PaymenrOrderAPI.Startup))]
namespace PaymenrOrderAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
