using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.Common
{
	public class PageList<T>
	{
		private PageList(List<T> items, int pageIndex, int pageSize, int totalCount)
		{
			Items = items;
			PageIndex= pageIndex;
			PageSize= pageSize;
			TotalCount=totalCount;
		}

		public List<T> Items { get; }
		public int PageIndex { get; }
		public int PageSize { get; }
		public int TotalCount { get; }


		public static PageList<T> Cretae(IQueryable<T> query, SearchBase search)
		{
			var totalCount = query.Count();
			var items = query.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToList();
			return new PageList<T>(items, search.PageIndex, search.PageSize, totalCount);
		}

	}
}
