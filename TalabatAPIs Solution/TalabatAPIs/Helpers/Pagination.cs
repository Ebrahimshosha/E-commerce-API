using TalabatAPIs.DTO;

namespace TalabatAPIs.Helpers
{
	public class Pagination<T>
	{

		public int Pagesize { get; set; }
		public int PageIndex { get; set; }
		public int Count { get; set; }
		public IReadOnlyList <T> Data { get; set; }	


		public Pagination(int pagesize, int pageIndex, IReadOnlyList<T> mappedProduct,int count)
		{
			Pagesize = pagesize;
			PageIndex = pageIndex;
			Data = mappedProduct;
			Count = count;
		}

	}
}
