using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore;
using TalabatCore.Entities;
using TalabatCore.Repositories;
using TalabatRebosatiory.Data;

namespace TalabatRebosatiory
{
    public class Unitofwork : IUnitofwork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _repository;


        public Unitofwork(StoreContext dbContext) // Ask CLR for creating Object from StoreContext
        {
            _dbContext = dbContext;
            _repository = new Hashtable();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;

            if (!_repository.ContainsKey(type))
            {
                var repository = new GenericRepositories<T>(_dbContext);
                _repository.Add(type, repository);
            }
            return _repository[type] as IGenericRepository<T>;
        }

        public async Task<int> Complete()
            => await _dbContext.SaveChangesAsync();


        public async ValueTask DisposeAsync()
           => await _dbContext.DisposeAsync();

    }
}
