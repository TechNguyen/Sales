using Microsoft.Extensions.Options;
using Sale.Domain;
using Sale.Domain.Entities;
using Sale.Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Repository.OriginRepository
{
	public class OriginRepository : Repository<Origin>, IRepository<Origin>
	{
		public OriginRepository(SaleContext options) : base(options)
		{

		}

	}
}
