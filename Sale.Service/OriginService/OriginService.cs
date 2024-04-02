using Sale.Domain.Entities;
using Sale.Repository.OriginRepository;
using Sale.Service.Core;
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
	}
}
