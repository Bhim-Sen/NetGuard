using BLL.IService;
using Common.JWT;
using DAL.Entity;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
	public class CommonService : ICommonService
	{
		private readonly IUnitOfWork unitOfWork;

		public CommonService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public async Task<TblUserStatus?> GetLoginUser(string token)
		{
			try
			{
				var userIdStr = await GetJwtToken.GetUserDataFromJWT(token);
				if (Guid.TryParse(userIdStr, out Guid userId))
				{
					var user = await unitOfWork.DbContext.TblUserStatuses.FirstOrDefaultAsync(e => e.Id == userId);
					return user;
				}
				else
				{
					return null;
				}
			}
			catch
			{
				return null;
			}
		}
	}
}
