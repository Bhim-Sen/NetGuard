using DAL.Entity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		public NetGuardDbContext DbContext { get; }
		private IDbContextTransaction? _transaction;  

		public UnitOfWork(NetGuardDbContext dbContext) => DbContext = dbContext;  

	

		public async Task BeginTransactionAsync()
		{
			_transaction = await DbContext.Database.BeginTransactionAsync();
		}
		public async Task SaveChangesAsync()
		{
			await DbContext.SaveChangesAsync();
		}

		public async Task CommitAsync()
		{
			if (_transaction != null)
			{
				await _transaction.CommitAsync();
				await _transaction.DisposeAsync();
			}
		}

		public async Task RollbackAsync()
		{
			if (_transaction != null)
			{
				await _transaction.RollbackAsync();
				await _transaction.DisposeAsync();
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_transaction != null)
				await _transaction.DisposeAsync();

			await DbContext.DisposeAsync();
		}

		//------------------------------------------------------

		 
	}






}
