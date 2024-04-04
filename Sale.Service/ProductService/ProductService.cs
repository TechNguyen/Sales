using Microsoft.AspNetCore.Mvc.Filters;
using Sale.Domain;
using Sale.Domain.Entities;
using Sale.Repository.BranchRepository;
using Sale.Repository.ProductRepository;
using Sale.Service.Common;
using Sale.Service.Core;
using Sale.Service.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.ProductService
{
	public class ProductService : Service<Product>, IProductService
	{
		private readonly IProductRepository _productRepository;

		private readonly IBranchRepository _branchRepository;
		//private readonly IOriginRepository _originRepository;
		public ProductService(IProductRepository productRepository,
			IBranchRepository branchRepository
			//IOriginRepository originRepository
			) : base(productRepository)
		{
			_branchRepository = branchRepository;
			//_originRepository = originRepository;
			_productRepository = productRepository;
		}

		public async Task<PageList<ProductDto>> GetDataByPage(ProductSearchDto searchDto)
		{
			try
			{
				var query = from q in _productRepository.GetQueryable()


							join  btbl in _branchRepository.GetQueryable() on q.BranchId equals btbl.Id into bt
							from b in bt.DefaultIfEmpty()
							select new ProductDto
							{
								ProdcutPrice = q.ProdcutPrice,
								ProductName = q.ProductName,
								ProductDescription = q.ProductDescription,
								ProductMaterial = q.ProductMaterial,
								BranchName = b.BranchName,
								ProductType = q.ProductType,
								comment = q.comment,
								views = q.views,
								ProductNumber = q.ProductNumber,
								ProductQuanlity = q.ProductQuanlity,
								ProductSold = q.ProductSold
							};

				if(searchDto != null)
				{
					if(!string.IsNullOrEmpty(searchDto.ProductName))
					{
						query = query.Where(x => x.ProductName.RemoveAccentsUnicode().ToLower().Contains(searchDto.ProductName.ToLower()));
					}
					if(!string.IsNullOrEmpty(searchDto.BranchId.ToString()))
					{
						query = query.Where(x => x.BranchId == searchDto.BranchId);	
					}
					if(!string.IsNullOrEmpty(searchDto.Origin.ToString()))
					{
						query = query.Where(x => x.OriginId == searchDto.Origin);	
					}
					
				}
				var items = PageList<ProductDto>.Cretae(query,searchDto);
				return items;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
