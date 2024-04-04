using Sale.Domain.Entities;
using Sale.Repository.Core;
using Sale.Repository.OriginRepository;
using Sale.Service.Common;
using Sale.Service.Core;
using Sale.Service.Dtos.OriginDto;
using Sale.Service.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.OriginService
{
	public class OriginService : Service<Origin>, IOriginService
	{

		private readonly IOriginRepository _originRepository;

		public OriginService(IOriginRepository originRepository) : base(originRepository)
		{
			_originRepository = originRepository;
		}

		public async Task<PageList<OriginDto>> GetDataByPage(OriginSearchDto searchDto)
		{
			try
			{
				var query = from q in _originRepository.GetQueryable()

							select new OriginDto
							{
								OriginName = q.OriginName
							};

				if (searchDto != null)
				{
					if (!string.IsNullOrEmpty(searchDto.OriginName))
					{
						query = query.Where(x => x.OriginName.RemoveAccentsUnicode().ToLower().Contains(searchDto.OriginName.ToLower()));
					}
				}
				var items = PageList<OriginDto>.Cretae(query, searchDto);
				return items;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
