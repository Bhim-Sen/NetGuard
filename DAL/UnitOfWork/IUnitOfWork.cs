using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
	public interface IUnitOfWork
	{
		NetGuardDbContext DbContext { get; }
 
 		void BeginTransaction();
		void Commit();
		void Rollback();
		void Savechanges();
	}
}
