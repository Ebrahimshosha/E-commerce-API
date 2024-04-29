using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Specifications;

namespace TalabatCore.Repositories
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		#region WitoutSpecification

		Task<IReadOnlyList<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);

		#endregion

		#region WithSpecification

		Task<IReadOnlyList<T>> GetAllWithSpecAsync(Ispecifications<T> spec);

		Task<T> GetByEntityWithSpecAsync(Ispecifications<T> spec);

		Task<int> GetCountWithSpecAsync(Ispecifications<T> spec);

		#endregion

		Task AddAsync(T item);

		void Delete(T item);
		void Update(T item);
	}
}
