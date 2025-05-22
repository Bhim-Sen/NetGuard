using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using NetGuardUI.Data.Extension; 
using NetGuardUI.Data;
using NetGuardUI.Interface;




namespace NetGuardUI.Pages.Identity
{
	[AllowAnonymous]
	public class LoginModel : PageModel
	{
		private readonly ILogin _login;
		private string _referralCode = "";


		private readonly WebsiteAuthenticator _websiteAuthenticator;


		public LoginModel(ILogin login, AuthenticationStateProvider stateProvider, WebsiteAuthenticator websiteAuthenticator)
		{
			_login = login;
			_websiteAuthenticator = websiteAuthenticator;
		}


		public IActionResult OnGetAsync(string returnUrl)
		{
			returnUrl = returnUrl != null ? "/" + returnUrl : "/";
			string provider = "Google";

			// Request a redirect to the external login provider.
			var authenticationProperties = new AuthenticationProperties
			{
				RedirectUri = Url.Page("./Login",
				pageHandler: "Callback",
				values: new { returnUrl }),
			};
			return new ChallengeResult(provider, authenticationProperties);
		}
		public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl, string? remoteError)
		{
			try
			{

				// The user canceled the OAuth flow
				if (remoteError != null) { return LocalRedirect("/"); }
				var GoogleUser = this.User.Identities.FirstOrDefault();
				if (returnUrl.Contains("ReferralCode"))
				{
					string pattern = @"/ReferralCode/([^/]+)";
					Regex regex = new Regex(pattern);
					Match match = regex.Match(returnUrl);
					_referralCode = match.Groups[1].Value;
					returnUrl = "/";
				}
				var redirectResult = await CheckUserIsValidandClaimSet(GoogleUser, returnUrl, _referralCode);
				return redirectResult ?? LocalRedirect(returnUrl ?? "/");

			}
			catch { return LocalRedirect("/"); }

		}
		public async Task<IActionResult?> CheckUserIsValidandClaimSet(ClaimsIdentity? GoogleUser, string? returnUrl, string? referralCode)
		{
			if (GoogleUser?.IsAuthenticated == true)
			{
				var dto = new UserSignInDto();
				var claims = GoogleUser.Claims.ToList();
				var data = SetData(claims);
				if (!string.IsNullOrWhiteSpace(referralCode))
				{
					data.ReferByUserCode = referralCode;
				}
				var token = await _login.GetUserToken(data);
				if (token.Token != null)
				{
					dto.Email = data.Email;
					dto.PhoneNumber = Convert.ToInt64(data.PhoneNumber);

					var claimData = await _websiteAuthenticator.SaveUserSessionzAsync(dto, token.Token);
					var claimDataJson = JsonConvert.SerializeObject(claimData);
					var encodedClaim = Uri.EscapeDataString(claimDataJson);
					var encodedReturnUrl = Uri.EscapeDataString(returnUrl ?? "/");
					// ?? Redirect to postlogin with query string
					return Redirect($"/postlogin?claim={encodedClaim}&redirectUrl={encodedReturnUrl}");
				}
			}

			return null;
		}


		public async Task CheckUserIsValidandClaimSettest(ClaimsIdentity? GoogleUser)
		{
			try
			{
				if (GoogleUser!.IsAuthenticated)
				{
					var dto = new UserSignInDto();
					var claims = GoogleUser.Claims.ToList();
					var data = SetData(claims);
					data.ReferByUserCode = _referralCode;
					var token = await _login.GetUserToken(data);
					if (token.Token != null)
					{
						dto.Email = data.Email; dto.PhoneNumber = Convert.ToInt64(data.PhoneNumber);
						var claimData = await _websiteAuthenticator.SaveUserSessionzAsync(dto, token.Token);
						var claimDataJson = JsonConvert.SerializeObject(claimData);
						var encoded = Uri.EscapeDataString(claimDataJson);

						Redirect($"/postlogin?claim={encoded}");
						return;

					}
					else
					{
					}

				}
			}
			catch
			{

			}



		}


		public static async Task<List<string>> DecodeJwt(string token)
		{
			var userRoles = new List<string>();

			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
			var rolesClaim = jsonToken!.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

			if (rolesClaim != null)
			{
				foreach (var role in rolesClaim)
				{
					var roles = role.Value.Split(',');
					userRoles.AddRange(roles);
				}
			}
			return userRoles;
		}
		public static UserDto SetData(List<Claim> claims)
		{
			var userDto = new UserDto();

			foreach (var claim in claims)
			{
				switch (claim.Type)
				{
					case ClaimTypes.Name:
						userDto.Name = claim.Value;
						break;
					case ClaimTypes.Email:
						userDto.Email = claim.Value;
						break;
					case "urn:google:image":// Assuming "picture" claim holds the URL of the user's image
						userDto.ImageUrl = claim.Value;
						break;
						// Add cases for other claim types as needed
				}
			}

			return userDto;
		}


	}
}
