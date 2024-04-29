﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatRebosatiory.Data.Configurations
{
	internal class ProductTypeConfig : IEntityTypeConfiguration<ProductType>
	{
		public void Configure(EntityTypeBuilder<ProductType> builder)
		{
			builder.Property(PT => PT.Name).IsRequired();
		}
	}
}
