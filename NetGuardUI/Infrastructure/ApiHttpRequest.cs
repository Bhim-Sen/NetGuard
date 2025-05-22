using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using BlazorBootstrap;
using Microsoft.JSInterop;
using NetGuardUI.Data;
using NetGuardUI.Data.Extension;
using NetGuardUI.Pages.Identity;

namespace NetGuardUI.Infrastructure
{
	public class ApiHttpRequest
	{

		private readonly WebsiteAuthenticator _customStateProvider;
		private string _token = "";
		private readonly LogoutModel _logoutModel; private readonly NavigationManager _navMagager;
		private readonly ToastService _toast;
		private readonly IJSRuntime JS;


		public ApiHttpRequest(LogoutModel logoutModel, NavigationManager navMagager, WebsiteAuthenticator customStateProvider, ToastService toastMessage, IJSRuntime js)
		{
			_customStateProvider = customStateProvider;
			_logoutModel = logoutModel;
			_navMagager = navMagager;
			_toast = toastMessage;
			JS = js;
		}
		enum apistatus { OK, Fail }
		public async Task<Response> httpRequestForGoogleAuth(dynamic parm, string apiUrl)
		{
			var response = new Response(); var jsonBody = JsonConvert.SerializeObject(parm); var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
			try
			{
				var httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
				var Jsonresponse = await httpClient.PostAsync(apiUrl, content);
				if (Jsonresponse.StatusCode == HttpStatusCode.Unauthorized)
				{
					//await Task.Run(() => { _navMagager.NavigateTo("/Logout"); });
					response.status = HttpStatusCode.Unauthorized.ToString();
					return response;
				}
				if (Jsonresponse.IsSuccessStatusCode) { response = JsonConvert.DeserializeObject<Response>(await Jsonresponse.Content.ReadAsStringAsync()); }
				else { Console.WriteLine($"Error: {Jsonresponse.StatusCode}"); response = new Response(); }
			}
			catch (Exception ex) { }
			return response;
		}
		public async Task<Response> httpRequest(dynamic parm, string apiUrl)
		{

			var response = new Response(); var jsonBody = JsonConvert.SerializeObject(parm); var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
			try
			{
				var httpClient = new HttpClient();
				//var auth = await _authenticationStateProvider.GetAuthenticationStateAsync();

				_token = await GetUserToken.GetTokenFromManualClaimAsync(_customStateProvider);
				if (_token == "")
				{
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
					var Jsonresponse = await httpClient.PostAsync(apiUrl, content);
					if (Jsonresponse.StatusCode == HttpStatusCode.Unauthorized)
					{
						_navMagager.NavigateTo("/Logout", true);
						_toast.Notify(new(ToastType.Danger, $"Session Expired, Login Again"));
					}
					if (Jsonresponse.IsSuccessStatusCode) { response = JsonConvert.DeserializeObject<Response>(await Jsonresponse.Content.ReadAsStringAsync()); }
					else { Console.WriteLine($"Error: {Jsonresponse.StatusCode}"); response = new Response(); }
				}
				else
				{
					bool isValid = await CheckUserHaveValidToken(_token);
					if (isValid)
					{
						httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
						var Jsonresponse = await httpClient.PostAsync(apiUrl, content);
						if (Jsonresponse.StatusCode == HttpStatusCode.Unauthorized)
						{
							_navMagager.NavigateTo("/Logout", true);
							_toast.Notify(new(ToastType.Danger, $"Session Expired, Login Again"));
						}

						if (Jsonresponse.IsSuccessStatusCode) { response = JsonConvert.DeserializeObject<Response>(await Jsonresponse.Content.ReadAsStringAsync()); }
						else { Console.WriteLine($"Error: {Jsonresponse.StatusCode}"); response = new Response(); }
					}
					else
					{
						_navMagager.NavigateTo("/Logout", true);
						_toast.Notify(new(ToastType.Danger, $"Session Expired, Login Again"));

					}
				}
			}
			catch (Exception ex)
			{
				_toast.Notify(new ToastMessage(ToastType.Danger, ex.Message));
				await JS.InvokeVoidAsync("console.error", $"[ERROR] {ex.Message}", ex.StackTrace);
			}
			return response;
		}
		public async Task<Response> httpGetRequest(string apiUrl)
		{
			try
			{
				var response = new Response();
				var httpClient = new HttpClient();
				//var auth = await _authenticationStateProvider.GetAuthenticationStateAsync();

				_token = await GetUserToken.GetTokenFromManualClaimAsync(_customStateProvider);
				if (_token == "")
				{
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
					var Jsonresponse = await httpClient.GetAsync(apiUrl);
					if (Jsonresponse.StatusCode == HttpStatusCode.Unauthorized)
					{
						_navMagager.NavigateTo("/Logout", true);
						_toast.Notify(new(ToastType.Danger, $"Session Expired, Login Again"));
					}

					if (Jsonresponse.IsSuccessStatusCode) { response = JsonConvert.DeserializeObject<Response>(await Jsonresponse.Content.ReadAsStringAsync()); }
					else { Console.WriteLine($"Error: {Jsonresponse.StatusCode}"); response = new Response(); }
				}
				else
				{
					bool isValid = await CheckUserHaveValidToken(_token);
					if (isValid)
					{
						httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
						var Jsonresponse = await httpClient.GetAsync(apiUrl);
						if (Jsonresponse.StatusCode == HttpStatusCode.Unauthorized)
						{

							_navMagager.NavigateTo("/Logout", true);
							_toast.Notify(new(ToastType.Danger, $"Session Expired, Login Again"));
						}
						if (Jsonresponse.IsSuccessStatusCode) { response = JsonConvert.DeserializeObject<Response>(await Jsonresponse.Content.ReadAsStringAsync()); }
						else { Console.WriteLine($"Error: {Jsonresponse.StatusCode}"); response = new Response(); }
					}
					else
					{

						_navMagager.NavigateTo("/Logout", true);
						_toast.Notify(new(ToastType.Danger, $"Session Expired, Login Again"));
					}
				}
				return response;
			}
			catch (Exception ex)
			{
				await JS.InvokeVoidAsync("console.error", $"[ERROR] {ex.Message}", ex.StackTrace);
				_toast.Notify(new ToastMessage(ToastType.Danger, ex.Message));
				throw;
			}


		}

		public static bool ApiResponseCheck(Response responseModel)
		{
			if (responseModel != null) { if (!string.IsNullOrEmpty(responseModel.status) && responseModel.status.Equals(apistatus.OK.ToString())) { return true; } else { return false; } } else { return false; }
		}
		public async Task<bool> CheckUserHaveValidToken(string jwtToken)
		{
			var handler = new JwtSecurityTokenHandler();
			if (jwtToken != "")
			{
				var token = handler.ReadJwtToken(jwtToken);
				var expirationTimeUnix = token.Payload.Exp;
				var expirationTimeUtc = DateTimeOffset.FromUnixTimeSeconds((long)expirationTimeUnix).UtcDateTime;
				var expirationTime = expirationTimeUtc.AddMinutes(-2);
				var currentTimeUtc = DateTime.UtcNow;

				if (expirationTime <= currentTimeUtc) { await _logoutModel.OnGetAsync(); return false; }
				return true;
			}

			return false;
		}
	}
}
