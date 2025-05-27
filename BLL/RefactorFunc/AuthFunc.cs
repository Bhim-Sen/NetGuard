using BLL.IService;
using Common;
using Common.Enum;
using Common.JWT;
using DAL.Entity;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.RefactorFunc
{
	public class AuthFunc
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly ICommonService _commonService;
		public AuthFunc(IUnitOfWork unitOfWork, ICommonService commonService)
		{
			this.unitOfWork = unitOfWork;
			_commonService = commonService;
		}

		public  async Task AddUserAsync(TblUser user)
		{
			await unitOfWork.DbContext.TblUsers.AddAsync(user);
			await unitOfWork.SaveChangesAsync();
		}
 
		public  async Task<TblUser?> AddNewUser(UserDTO userCredentials, TblUser? user)
		{
			await AddUserAsync(await Mapper.MapToDbTbl<UserDTO, TblUser>(userCredentials));// Add new user 

			if (!string.IsNullOrEmpty(userCredentials.Email))
			{
				user = await unitOfWork.DbContext.TblUsers.FirstOrDefaultAsync(f => f.Gmail == userCredentials.Email);
			}
			else if (userCredentials.PhoneNumber != null)
			{
				user = await unitOfWork.DbContext.TblUsers!.FirstOrDefaultAsync(p => p.PhoneNumber == userCredentials.PhoneNumber);
			}
			return user;
		}

		public  async Task<UserDTO> PopulateUserRoles(UserDTO userDto, TblUser user)
		{
			userDto = await Mapper.MapToDto<TblUser, UserDTO>(user!);
			var roles = new List<UserRole> { UserRole.User };
			if (userDto.IsAdmin == true) roles.Add(UserRole.Admin);
			if (userDto.IsSuperAdmin == true) roles.Add(UserRole.SuperAdmin);
			if (userDto.IsInBusiness == true) roles.Add(UserRole.Vendor);
			if (userDto.IsManager == true) roles.Add(UserRole.Manager);
			userDto.UserRole = string.Join(",", roles.Select(r => r.ToString()));
			return userDto;
		}


	}
}
