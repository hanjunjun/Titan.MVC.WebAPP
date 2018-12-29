using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Titan.WebMVC.Startup))]
namespace Titan.WebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
