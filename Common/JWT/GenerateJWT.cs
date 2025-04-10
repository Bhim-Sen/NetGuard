using Common.CommonMethods;
using Common.DTO;
 
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.JWT
{
	public class GenerateJWT
	{
		public static Task<JwtDto> GenerateJWToken(UserDTO? userCredentials, IConfiguration configuration)
		{
			List<Claim>? claims = null;
			var roles = userCredentials!.UserRole!.Split(",");
			var tokenDto = new JwtDto();
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			if (userCredentials.PhoneNumber != null)
			{
				claims = new List<Claim> {
						new Claim(JwtRegisteredClaimNames.UniqueName, userCredentials!.Name!),
						new Claim(JwtRegisteredClaimNames.NameId, userCredentials!.Id.ToString()!),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
					};
			}
			else
			{
				claims = new List<Claim> {
						new Claim(JwtRegisteredClaimNames.UniqueName, userCredentials!.PhoneNumber!.ToString()!),
						new Claim(JwtRegisteredClaimNames.NameId, userCredentials!.Id!.ToString()!),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
					};
			}
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
			}

			var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
				configuration["Jwt:Issuer"],
				claims,
				expires: IndianTimeZone.GetIndianTimeZone().AddYears(2),
				signingCredentials: credentials);
			var jwtHandler = new JwtSecurityTokenHandler();
			tokenDto.Token = jwtHandler.WriteToken(token);

			return Task.FromResult(tokenDto);
		}
	}
	public class JwtDto
	{
		public string? Token { get; set; }
	}
	public class UserDTO
	{
		public Guid? Id { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Name { get; set; }
		public string? UserRole { get; set; }
	}
}
