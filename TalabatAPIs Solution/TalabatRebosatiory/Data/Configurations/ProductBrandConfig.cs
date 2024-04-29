using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatRebosatiory.Data.Configurations
{
	public class ProductBrandConfig : IEntityTypeConfiguration<ProductBrand>
	{

		public void Configure(EntityTypeBuilder<ProductBrand> builder)
		{
			builder.Property(Pb => Pb.Name).IsRequired();
		}
	}
}
