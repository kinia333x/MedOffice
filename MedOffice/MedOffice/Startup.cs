using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedOffice.Startup))]
namespace MedOffice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
