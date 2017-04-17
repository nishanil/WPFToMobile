using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MyExpenses.Helpers;
using MyExpenses.Models;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using MyExpenses.Stores;

namespace MyExpenses.Services
{
	public interface IAuthenticator
	{
		Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> paramameters = null);
		Task<bool> RefreshUser(IMobileServiceClient client);
		void ClearCookies();
	}

	public abstract class BaseSocialAuthenticator : IAuthenticator
	{
		public virtual void ClearCookies()
		{

		}

		public abstract Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> paramameters = null);


		public virtual async Task<bool> RefreshUser(IMobileServiceClient client)
		{
			try
			{
				var user = await client.RefreshUserAsync();

				if (user != null)
				{
					client.CurrentUser = user;
					Settings.AuthToken = user.MobileServiceAuthenticationToken;
					return true;
				}
			}
			catch (System.Exception e)
			{
				Debug.WriteLine("Unable to refresh user: " + e);
			}

			return false;
		}
	}
    
	public class AuthenticationHandler : DelegatingHandler
	{


		public static MobileServiceAuthenticationProvider ProviderType =>
			MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory;

		private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
		private static bool isReauthenticating = false;
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			//Clone the request in case we need to send it again
			var clonedRequest = await CloneRequest(request);
			var response = await base.SendAsync(clonedRequest, cancellationToken);

			//If the token is expired or is invalid, then we need to either refresh the token or prompt the user to log back in
			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				if (isReauthenticating)
					return response;

				var client = DependencyService.Get<IDataStore<Item>>() as AzureDataStore<Item>;

				string authToken = client.MobileService.CurrentUser.MobileServiceAuthenticationToken;
				await semaphore.WaitAsync();
				//In case two threads enter this method at the same time, only one should do the refresh (or re-login), the other should just resend the request with an updated header.
				if (authToken != client.MobileService.CurrentUser.MobileServiceAuthenticationToken)  // token was already renewed
				{
					semaphore.Release();
					return await ResendRequest(client.MobileService, request, cancellationToken);
				}

				isReauthenticating = true;
				bool gotNewToken = false;
				try
				{

					gotNewToken = await RefreshToken(client.MobileService);


					//Otherwise if refreshing the token failed or Facebook\Twitter is being used, prompt the user to log back in via the login screen
					if (!gotNewToken)
					{
						gotNewToken = await Login(client.MobileService);
					}
				}
				catch (System.Exception e)
				{
					Debug.WriteLine("Unable to refresh token: " + e);
				}
				finally
				{
					isReauthenticating = false;
					semaphore.Release();
				}


				if (gotNewToken)
				{
					if (!request.RequestUri.OriginalString.Contains("/.auth/me"))   //do not resend in this case since we're not using the return value of auth/me
					{
						//Resend the request since the user has successfully logged in and return the response
						return await ResendRequest(client.MobileService, request, cancellationToken);
					}
				}
			}

			return response;
		}


		private async Task<HttpResponseMessage> ResendRequest(IMobileServiceClient client, HttpRequestMessage request, CancellationToken cancellationToken)
		{
			// Clone the request
			var clonedRequest = await CloneRequest(request);

			// Set the authentication header
			clonedRequest.Headers.Remove("X-ZUMO-AUTH");
			clonedRequest.Headers.Add("X-ZUMO-AUTH", client.CurrentUser.MobileServiceAuthenticationToken);

			// Resend the request
			return await base.SendAsync(clonedRequest, cancellationToken);
		}

		private async Task<bool> RefreshToken(IMobileServiceClient client)
		{
			var authentication = DependencyService.Get<IAuthenticator>();
			if (authentication == null)
			{
				throw new InvalidOperationException("Make sure the ServiceLocator has an instance of IAuthenticator.");
			}

			try
			{
				return await authentication.RefreshUser(client);
			}
			catch (System.Exception e)
			{
				Debug.WriteLine("Unable to refresh user: " + e);
			}

			return false;
		}

		private async Task<bool> Login(IMobileServiceClient client)
		{
			var authentication = DependencyService.Get<IAuthenticator>();
			if (authentication == null)
			{
				throw new InvalidOperationException("Make sure the ServiceLocator has an instance of IAuthenticator.");
			}

			var accountType = ProviderType;
			var parameters = App.LoginParameters;
			try
			{
				var user = await authentication.LoginAsync(client, accountType, parameters);
				if (user != null)
					return true;
			}
			catch (System.Exception e)
			{
			}

			return false;
		}

		private async Task<HttpRequestMessage> CloneRequest(HttpRequestMessage request)
		{
			var result = new HttpRequestMessage(request.Method, request.RequestUri);
			foreach (var header in request.Headers)
			{
				result.Headers.Add(header.Key, header.Value);
			}

			if (request.Content != null && request.Content.Headers.ContentType != null)
			{
				var requestBody = await request.Content.ReadAsStringAsync();
				var mediaType = request.Content.Headers.ContentType.MediaType;
				result.Content = new StringContent(requestBody, Encoding.UTF8, mediaType);
				foreach (var header in request.Content.Headers)
				{
					if (!header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
					{
						result.Content.Headers.Add(header.Key, header.Value);
					}
				}
			}

			return result;
		}
	}
}
