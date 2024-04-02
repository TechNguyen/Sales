using Sale.Domain.Entities;
using Sale.Repository.BranchRepository;
using Sale.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.ProductService
{
	public class BranchService : Service<Branch>,IBranchService 
	{
		private readonly IBranchRepository _branchRepository;
		public BranchService(IBranchRepository branchRepository) : base (branchRepository)
		{
			_branchRepository = branchRepository;
		}
	}
}
