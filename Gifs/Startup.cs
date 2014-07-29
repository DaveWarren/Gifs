using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gifs.Startup))]
namespace Gifs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
