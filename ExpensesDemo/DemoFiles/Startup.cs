using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Configuration;

[assembly: OwinStartup(typeof(Expenses.Wcf.Startup))]
namespace Expenses.Wcf
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            WindowsAzureActiveDirectoryBearerAuthenticationOptions options = new WindowsAzureActiveDirectoryBearerAuthenticationOptions()
            {
                Tenant = ConfigurationManager.AppSettings["aad:Audience"],
                TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidAudience = ConfigurationManager.AppSettings["aad:Audience"]
                },
            };

            app.UseWindowsAzureActiveDirectoryBearerAuthentication(options);
        }
    }
}