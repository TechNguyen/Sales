using Sale.Domain.Entities;
using Sale.Repository.BranchRepository;
using Sale.Repository.Core;
using Sale.Service.Common;
using Sale.Service.Core;
using Sale.Service.Dtos.BranchDto;
using Sale.Service.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.BranchService
{
	public class BranchService : Service<Branch>, IBranchService
	{


		private readonly IBranchRepository _branchRepository;
        public BranchService(IRepository<Branch> repository, IBranchRepository branchRepository) : base(repository)
		{

			_branchRepository = branchRepository;
		}

		public async Task<PageList<BranchDto>> GetDataByPage(BranchSearchDto searchDto)
		{
			try
			{
				var query =	from q in _branchRepository.GetQueryable()

								select new BranchDto
								{
									BranchName = q.BranchName
								};

				if (searchDto != null)
				{
					if (!string.IsNullOrEmpty(searchDto.BranchName))
					{
						query = query.Where(delegate (BranchDto b)
						{
							return b.BranchName.RemoveAccentsUnicode().ToLower().Contains(searchDto.BranchName.ToLower());
						}).AsQueryable();
					}
					if(searchDto.PageIndex == null || searchDto.PageIndex <= 0)
					{
						searchDto.PageIndex = 1;
					}
					if (searchDto.PageSize == null || searchDto.PageSize <= 0)
					{
						searchDto.PageSize = 10;
					}
				} else
				{
					query = query.OrderByDescending(a => a.BranchName);
				}
				var items = PageList<BranchDto>.Cretae(query, searchDto);
				return items;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
