using Sale.Domain.Entities;
using Sale.Repository.CommentsRepository;
using Sale.Repository.Core;
using Sale.Service.Common;
using Sale.Service.Core;
using Sale.Service.Dtos.CommentDto;
using Sale.Service.Dtos.RateDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.CommentsService
{
	public class CommentsService : Service<Comments>, ICommentsService
	{
		private readonly ICommentsRepository _commentsRepository;
		public CommentsService(IRepository<Comments> repository, ICommentsRepository commentsRepository) : base(commentsRepository)
		{
			_commentsRepository = commentsRepository;
		}

		public PageList<CommentsDto> GetByProduct(Guid prroductId, CommentsSearchDto searchDto)
		{
			try
			{
				var query = from q in _commentsRepository.GetQueryable()
							where q.ProductId == prroductId
							select new CommentsDto
							{
								ProductId = q.ProductId,
								UserId = q.UserId,
								createAt = q.CreatedDate,
								Comment = q.Comment,
							};
				if (searchDto.PageSize == null && searchDto.PageSize <= 0)
				{
					searchDto.PageSize = 10;
				}
				if (searchDto.PageIndex == null && searchDto.PageIndex <= 0)
				{
					searchDto.PageIndex = 1;
				}
				var data = PageList<CommentsDto>.Cretae(query, searchDto);
				return data;
			}
			catch (Exception e)
			{
				return null;
			}
		}
	}
}
