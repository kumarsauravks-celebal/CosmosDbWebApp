using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CosmosDbWebApp.Startup))]
namespace CosmosDbWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
