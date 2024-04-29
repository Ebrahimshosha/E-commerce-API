using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Specifications;

namespace TalabatRebosatiory
{
	public static class SpecificationEvaluator<T> where T : BaseEntity
	{
		// Function To build Query
		public static IQueryable<T> GetQuery (IQueryable<T> inputQuery,Ispecifications<T> spec)
		{
			var Query = inputQuery; // _dbcontext.Set<T>()

			if (spec.Criteria != null)
			{
				Query = Query.Where(spec.Criteria); // _dbcontext.Set<T>().Where(x=>x.Id == id)
			}

			if(spec.OrderBy != null)
			{
				Query = Query.OrderBy(spec.OrderBy);
			}
			if(spec.OrderBydesc != null)
			{
				Query = Query.OrderByDescending(spec.OrderBydesc);
			}
			if(spec.IsPaginationEnable)
			{
				Query=Query.Skip(spec.Skip).Take(spec.Take);
			}

			Query = spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
			
			// _dbcontext.Products.Include(p=>p.ProductBrand).Include(p=>p.ProductType)
			
			return Query ;
		}
	}
}
