using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IService
{
	public interface ICommonService
	{
		Task<TblUserStatus?> GetLoginUser(string token);
	}
}
