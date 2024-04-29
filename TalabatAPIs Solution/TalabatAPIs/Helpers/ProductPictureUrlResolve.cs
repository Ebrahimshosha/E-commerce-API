﻿using AutoMapper;
using TalabatAPIs.DTO;
using TalabatCore.Entities;

namespace TalabatAPIs.Helpers
{
	public class ProductPictureUrlResolve : IValueResolver<Product, ProductToReturnDto, string>
	{
		private readonly IConfiguration _configuration;

		public ProductPictureUrlResolve(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
		{
			if(!string.IsNullOrEmpty(source.PictureUrl))
			{
				return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
			}
			return string.Empty;
		}
	}
}