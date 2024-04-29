using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.Specifications
{
	public class ProductWithFilterationForCountSpec:BaseSpecifications<Product>
	{
        public ProductWithFilterationForCountSpec(ProductSpecParams Params)
			: base(p =>
			(string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))
			&&
			(!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)
			&&
			(!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId))
		{
            
        }
    }
}
