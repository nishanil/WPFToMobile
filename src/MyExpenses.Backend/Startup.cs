using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyExpenses_SIService.Startup))]

namespace MyExpenses_SIService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}