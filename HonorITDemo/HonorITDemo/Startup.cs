using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HonorITDemo.Startup))]
namespace HonorITDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
