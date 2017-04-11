using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.WPF
{
    public class AADSignIn
    {
        private static string aadInstance = ConfigurationManager.AppSettings["aad:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["aad:Tenant"];
        private static string clientId = ConfigurationManager.AppSettings["aad:ClientId"];
        private static Uri redirectUri = new Uri(ConfigurationManager.AppSettings["aad:RedirectUri"]);
        private static string serviceResourceId = ConfigurationManager.AppSettings["aad:ResourceId"];

        private static string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        public static AuthenticationResult AADAuthResult { get; set; }

        public static void SignIn()
        {
            AuthenticationContext authContext = new AuthenticationContext(authority, new FileCache());
            try
            {
                AADSignIn.AADAuthResult = authContext.AcquireToken(serviceResourceId, clientId, redirectUri, PromptBehavior.Auto);
            }
            catch (AdalException ex)
            {
                // No token in cache
                if (ex.ErrorCode == "user_interaction_required")
                {
                    // TODO: prompt user to sign in
                }
                else
                {
                    // TODO: unexpected error
                }
            }
        }
    }
}
