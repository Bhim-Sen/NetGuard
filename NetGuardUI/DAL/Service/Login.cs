using NetGuardUI.Data;
using NetGuardUI.Infrastructure;
using NetGuardUI.Interface;
using Newtonsoft.Json;

namespace NetGuardUI.Service
{
	public class Login : ILogin
	{
		private readonly ApiHttpRequest _apiHttpRequest;

		public Login(ApiHttpRequest apiHttpRequest)
		{
			this._apiHttpRequest = apiHttpRequest;
		}
		public async Task<Result> GetUserToken(UserDto userDto)
		{
			if (userDto.Email != "")
			{

				var apiUrl = ApiURL.Login;
				var response = await httpRequestForUserToken(userDto, apiUrl);
				if (ApiHttpRequest.ApiResponseCheck(response)) { return JsonConvert.DeserializeObject<Result>(response.result); }
				else { return new Result(); }
			}
			else
			{ return new Result(); }
		}
		public async Task<Response> httpRequestForUserToken(dynamic parm, string apiUrl)
		{
			var response = await _apiHttpRequest.httpRequestForGoogleAuth(parm, apiUrl);
			return response;
		}



		async Task<Response> ILogin.LogInMethod(UserLoginDto userDto)
		{
			var response = new Response();
			try
			{
				var apiUrl = ApiURL.Login;
				response = await _apiHttpRequest.httpRequest(userDto, apiUrl);
			}
			catch (HttpRequestException ex)
			{
				response = new Response();
			}
			return response;
		}






	}
}
