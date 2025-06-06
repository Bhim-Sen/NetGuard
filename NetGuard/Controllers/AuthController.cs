using BLL.IService;
using Common.JWT;
using MediaBrowser.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetGuard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _auth;
		public AuthController(IAuthService auth)
		{
			_auth = auth;
		}


		[HttpPost("Login")]
		public async Task<IActionResult> Login(UserDTO dto)
		{
			var user = await _auth.Login(dto);
 			return Ok(user);
		}	
		
		[HttpPost("LastSignOut")]
		public async Task<IActionResult> LastSignOut(UserDTO dto)
		{
			string jwtToken = await GetJwtToken.GetTokenFromHeader(HttpContext.Request);
			var user = await _auth.LastSignOut(jwtToken);
 			return Ok(user);
		}
	}
}
