using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using MyExpenses.MobileAppService.DataObjects;
using MyExpenses.MobileAppService.Models;
using Owin;

namespace MyExpenses.MobileAppService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new templateitemsInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<templateitemsContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);

        }
    }

    public class templateitemsInitializer : CreateDatabaseIfNotExists<MasterDetailContext>
    {
        protected override void Seed(MasterDetailContext context)
        {
            List<Item> items = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is a nice description"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is a nice description"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is a nice description"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is a nice description"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is a nice description"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is a nice description"},
            };

            foreach (Item item in items)
            {
                context.Set<Item>().Add(item);
            }

            base.Seed(context);
        }
    }
}

