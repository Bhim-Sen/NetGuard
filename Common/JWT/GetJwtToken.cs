using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.JWT
{
	public class GetJwtToken
	{
		public static Task<string> GetTokenFromHeader(HttpRequest? request)
		{
			string token = "";
			string authorizationHeader = request!.Headers["Authorization"]!;
			if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
			{
				token = authorizationHeader.Substring("Bearer ".Length).Trim();
			}
			return Task.FromResult(token);
		}

		public static Task<string> GetUserDataFromJWT(string jwtToken)
		{
			return Task.Run(() =>
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var token = tokenHandler.ReadJwtToken(jwtToken);
				Claim nameidClaim = token.Claims.FirstOrDefault(c => c.Type == "nameid")!;
				return nameidClaim?.Value ?? string.Empty;
			});
		}
	}
}
