using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tiplr.WebMVC.Startup))]
namespace Tiplr.WebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
