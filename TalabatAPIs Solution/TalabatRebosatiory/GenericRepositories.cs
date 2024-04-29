using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Repositories;
using TalabatCore.Specifications;
using TalabatRebosatiory.Data;

namespace TalabatRebosatiory
{
    public class GenericRepositories<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;

        public GenericRepositories(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region WitoutSpecification

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))
            //{
            //    return (IReadOnlyList<T>)await _dbcontext.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            //}
            return await _dbcontext.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            // return await _dbcontext.Set<T>().Where(x=>x.Id == id).FirstOrDefaultAsync();

            return await _dbcontext.Set<T>().FindAsync(id);

        }

        #endregion

        #region WithSpecification

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(Ispecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByEntityWithSpecAsync(Ispecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        #endregion

        public async Task<int> GetCountWithSpecAsync(Ispecifications<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(Ispecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
        }

        public async Task AddAsync(T item)
        {
            await _dbcontext.Set<T>().AddAsync(item);
        }

        public void Update(T item)
        {
            _dbcontext.Set<T>().Update(item);
        }
        public void Delete(T item)
        {
            _dbcontext.Set<T>().Remove(item);
        }


    }
}
