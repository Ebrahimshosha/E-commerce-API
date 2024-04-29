using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Entities.Order_Aggregation;

namespace TalabatRebosatiory.Data
{
	public static class StoreContextseed
	{
		// seeding
		public static async Task seedAsync(StoreContext dbcontext)
		{
			if (!dbcontext.ProductBrands.Any())
			{
				// seeding brands

				var brandsData = File.ReadAllText("../TalabatRebosatiory/Data/Dataseed/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);


				if (brands?.Count() > 0)
				{
					//var brands2 = brands.Select(b => new ProductBrand()
					//{
					//	Name = b.Name
					//}); // This if U don't want to delete id from json files

					foreach (var brand in brands)
					{
						await dbcontext.Set<ProductBrand>().AddAsync(brand);
					}
					dbcontext.SaveChanges();
				}
			}

			if (!dbcontext.ProductTypes.Any())
			{
				// seeding types

				var typesData = File.ReadAllText("../TalabatRebosatiory/Data/Dataseed/types.json");
				var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

				if (types?.Count() > 0)
				{
					foreach (var type in types)
					{
						await dbcontext.Set<ProductType>().AddAsync(type);
					}
					dbcontext.SaveChanges();
				}
			}

			if (!dbcontext.Products.Any())
			{
				// seeding products

				var productData = File.ReadAllText("../TalabatRebosatiory/Data/Dataseed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productData);

				if (products?.Count() > 0)
				{
					foreach (var product in products)
					{
						await dbcontext.Set<Product>().AddAsync(product);
					}
					dbcontext.SaveChanges();
				}
			}

			if (!dbcontext.DeliveryMethods.Any())
			{
				var DeliveryMethodData = File.ReadAllText("../TalabatRebosatiory/Data/Dataseed/delivery.json");
				var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);

				if (DeliveryMethods?.Count() > 0)
				{
					foreach (var DeliveryMethod in DeliveryMethods)
					{
						await dbcontext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
					}
					dbcontext.SaveChanges();
				}
			}
		}

	}
}
