using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatCore.Specifications
{
	public class ProductSpecParams
	{
		public string? Sort { get; set; }
		public int? TypeId { get; set; }
		public int? BrandId { get; set; }
		public int PageIndex { get; set; } = 1;



		private const int MaxPageSize = 10;

		private int pageSize=10; // Default

		public int Pagesize
		{
			get { return pageSize; }
			set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
		}



		private string? search;

		public string? Search
		{
			get { return search; }
			set { search = value?.ToLower()??string.Empty; }
		}


	}
}
