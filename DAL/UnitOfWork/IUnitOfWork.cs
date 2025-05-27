using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
	public interface IUnitOfWork :  IAsyncDisposable
	{
		NetGuardDbContext DbContext { get; }


		Task BeginTransactionAsync();
		Task CommitAsync();
		Task RollbackAsync();
		Task SaveChangesAsync();

		//-----------------------------
		 
	}
}
