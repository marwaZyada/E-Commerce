using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) ==typeof( Product))
          
                return await _dbcontext.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync() as IReadOnlyList<T>;
            return await _dbcontext.Set<T>().ToListAsync();
        }

       

        public async Task<T> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Product))

                return await _dbcontext.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync(p=>p.Id== id) as T;
            return await _dbcontext.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
        }

        public async Task Add(T entity)
        =>await _dbcontext.Set<T>().AddAsync(entity);

        public void Delete(T entity)
        => _dbcontext.Set<T>().Remove(entity);

        public void Update(T entity)
        => _dbcontext.Set<T>().Update(entity);
    }
}
