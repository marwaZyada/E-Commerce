using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;

        private Hashtable _Repositories;

       

        public UnitOfWork(StoreContext context)
        {
            _context = context;
           _Repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repo<TEntity>() where TEntity : BaseEntity
        {
            var type= typeof(TEntity).Name;
            if (!_Repositories.ContainsKey(type)) {
                var repository=new GenericRepository<TEntity>(_context);
                _Repositories.Add(type,repository);
                    }
            return _Repositories[type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }
        public async ValueTask DisposeAsync()
       =>await _context.DisposeAsync();

        
    }
}
