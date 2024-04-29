using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.Specifications
{
	public interface Ispecifications<T> where T : BaseEntity
	{
		// Signature for property for where condition
		public Expression<Func<T,bool>> Criteria { get; set; }

		// Signature for property for list od Include
		public List<Expression<Func<T, object>>> Includes { get; set; }

		// Signature for property for order by
		public Expression<Func<T, object>> OrderBy { get; set; }

		// Signature for property for order by desc
		public Expression<Func<T, object>> OrderBydesc { get; set; }

		// Signature for property for Skip
		public int Skip { get; set; }
		
		// Signature for property for Take
		public int Take { get; set; }
		
		// Signature for property for IsPaginationEnable
		public bool IsPaginationEnable { get; set; }


	}
}
