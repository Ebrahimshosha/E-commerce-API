using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.Specifications
{
	public class BaseSpecifications<T> : Ispecifications<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>> Criteria { get ; set ; }
		public List<Expression<Func<T, object>>> Includes { get; set ; } = new List<Expression<Func<T, object>>> ();
		public Expression<Func<T, object>> OrderBy { get; set; }
		public Expression<Func<T, object>> OrderBydesc { get; set; }
		public int Skip { get; set; }
		public int Take { get; set; }
		public bool IsPaginationEnable { get; set; }


		// Get All
		public BaseSpecifications()
        {
            
        }

        // Get by Id 
        public BaseSpecifications(Expression<Func<T, bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }

        public void AddOrderBy(Expression<Func<T, object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }

		public void AddOrderBydesc(Expression<Func<T, object>> OrderBydescExpression)
		{
			OrderBydesc = OrderBydescExpression;
		}
		public void ApplyPagenation(int skip,int take)
		{
			IsPaginationEnable = true;
			Skip = skip;
			Take = take;
		}
	}
}
