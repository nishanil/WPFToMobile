using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyExpenses.MobileAppService.Startup))]

namespace MyExpenses.MobileAppService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}