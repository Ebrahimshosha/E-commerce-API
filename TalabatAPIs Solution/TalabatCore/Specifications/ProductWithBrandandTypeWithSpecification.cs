using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.Specifications
{
	public class ProductWithBrandandTypeWithSpecification : BaseSpecifications<Product>
	{
		// Get All product
		public ProductWithBrandandTypeWithSpecification(ProductSpecParams Params)
			: base(p =>
			(string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))
			&&
			(!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)
			&&
			(!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId))
		{
			Includes.Add(p => p.ProductBrand);
			Includes.Add(p => p.ProductType);

			AddOrderBy(p => p.Name);


            if (!string.IsNullOrEmpty(Params.Sort))
			{
				switch (Params.Sort)
				{
					case "PriceAsc":
						AddOrderBy(p => p.Price);
						//OrderBy = p => p.Price;
						break;
					case "PriceDesc":
						AddOrderBydesc(p => p.Price);
						break;
					default:
						AddOrderBy(p => p.Name);
						break;
				}
			}

			ApplyPagenation(Params.Pagesize * (Params.PageIndex - 1), Params.Pagesize);
		}

		// Get products by id
		public ProductWithBrandandTypeWithSpecification(int id) : base(p => p.Id == id)
		{
			Includes.Add(p => p.ProductBrand);
			Includes.Add(p => p.ProductType);
		}
	}
}
