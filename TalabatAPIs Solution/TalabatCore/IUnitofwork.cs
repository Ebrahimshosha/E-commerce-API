using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Repositories;

namespace TalabatCore
{
    public interface IUnitofwork : IAsyncDisposable
    {
        public IGenericRepository<T> Repository<T>() where T : BaseEntity;

        Task<int> Complete();
    }
}
