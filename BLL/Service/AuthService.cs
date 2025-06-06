using Azure.Core;
using BLL.IService;
using BLL.RefactorFunc;
using Common;
using Common.CommonMethods;
using Common.Enum;
using Common.JWT;
using DAL.Entity;
using DAL.UnitOfWork;
using MediaBrowser.Model.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
	public class AuthService : IAuthService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly ICommonService _commonService;
		private readonly AuthFunc authFunc;

		public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, ICommonService commonService, AuthFunc authFunc)
		{
			this.unitOfWork = unitOfWork;
			_configuration = configuration;
			_commonService = commonService;
			this.authFunc = authFunc;
		}

		public async Task<Response> Login(UserDTO userDto)
		{
			await unitOfWork.BeginTransactionAsync();
			try
			{
				DateTime currentDateTime = IndianTimeZone.GetIndianTimeZone();

				var user = await unitOfWork.DbContext.TblUsers.FirstOrDefaultAsync(u => u.PhoneNumber == userDto.PhoneNumber || u.Gmail == userDto.Email);

				if (user == null)
					user = await authFunc.AddNewUser(userDto, user);
				if (user == null)
					return new Response(HttpStatusCode.BadRequest.ToString(), Message.CreationFailed);

				if (user != null && user.PasswordHash == userDto.Password)
				{
					userDto = await authFunc.PopulateUserRoles(userDto, user);
					var token = await GenerateJWT.GenerateJWToken(userDto, _configuration);
					await unitOfWork.CommitAsync();
					return new Response(HttpStatusCode.OK.ToString(), Message.Login, JsonConvert.SerializeObject(token));
				}
				else
				{
					return new Response(HttpStatusCode.Unauthorized.ToString(), Message.Error);
				}
			}
			catch (Exception ex)
			{
				await unitOfWork.RollbackAsync();
				return new Response(HttpStatusCode.InternalServerError.ToString(), ex.Message);
			}
		}

		public async Task<Response> LastSignOut(string token)
		{
			await unitOfWork.BeginTransactionAsync();
			try
			{
				var user = await _commonService.GetLoginUser(token);
				if (user == null)
				{
					await unitOfWork.RollbackAsync();
					return new Response(HttpStatusCode.BadRequest.ToString(), "Failed to Sign Out. User Is Null");
				}
				//user.LastLogout = IndianTimeZone.GetIndianTimeZone();
 				await unitOfWork.SaveChangesAsync();
				await unitOfWork.CommitAsync();
				return new Response(HttpStatusCode.OK.ToString(), "User Logout Successfully");
			}
			catch (Exception ex)
			{
				await unitOfWork.RollbackAsync();
				return new Response(HttpStatusCode.InternalServerError.ToString(), ex.Message);
			}
		}

	}
}
