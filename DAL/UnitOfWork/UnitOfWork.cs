using DAL.Entity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		public NetGuardDbContext DbContext { get; }
		private IDbContextTransaction? _transaction;  

		public UnitOfWork(NetGuardDbContext dbContext) => DbContext = dbContext;  

		public void BeginTransaction()
		{
			_transaction = DbContext.Database.BeginTransaction();
		}

		public void Commit()
		{
			_transaction?.Commit();
			_transaction?.Dispose();
		}

		public void Rollback()
		{
			_transaction?.Rollback();
			_transaction?.Dispose();
		}

		public void SaveChanges()
		{
			DbContext.SaveChanges();
		}

		void IUnitOfWork.Savechanges()
		{
			SaveChanges();
		}

		public void Dispose()
		{
			_transaction?.Dispose();
			DbContext.Dispose();
		}
	}






}
