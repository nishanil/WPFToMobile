using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Foundation;

using MyExpenses.Services;

using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyExpenses.iOS.Authentication.SocialAuthenticator))]
namespace MyExpenses.iOS.Authentication
{
	public class SocialAuthenticator : BaseSocialAuthenticator
	{
		public override async Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
		{
			try
			{
				var window = UIKit.UIApplication.SharedApplication.KeyWindow;
				var root = window.RootViewController;
				if (root == null)
					return null;

				var current = root;
				while (current.PresentedViewController != null)
				{
					current = current.PresentedViewController;
				}

				return await client.LoginAsync(current, provider, parameters);

			}
			catch (Exception e)
			{
				e.Data["method"] = "LoginAsync";
			}

			return null;
		}

		public override void ClearCookies()
		{
			var store = NSHttpCookieStorage.SharedStorage;
			var cookies = store.Cookies;

			foreach (var c in cookies)
			{
				store.DeleteCookie(c);
			}
		}
	}
}