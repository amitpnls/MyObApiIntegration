using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HonourIT_Xero_MyOB.Startup))]
namespace HonourIT_Xero_MyOB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
