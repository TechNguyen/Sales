using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using Sale.Domain;
using Sale.Domain.Entities;
using Sale.Repository.BranchRepository;
using Sale.Repository.FileImageRepository;
using Sale.Repository.OriginRepository;
using Sale.Repository.ProductRepository;
using Sale.Service.Common;
using Sale.Service.Core;
using Sale.Service.Dtos.FileImageDto;
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
		private readonly IOriginRepository _originRepository;
		private readonly IFileImageRepository _fileImageRepository;

		private readonly SaleContext _context;
		public ProductService(IProductRepository productRepository,
			IBranchRepository branchRepository,
			IOriginRepository originRepository,
			SaleContext context,
			IFileImageRepository fileImageRepository
			) : base(productRepository)
		{
			_fileImageRepository = fileImageRepository;
			_branchRepository = branchRepository;
			_context = context;
			_originRepository = originRepository;
			_productRepository = productRepository;
		}

		public ProductDetailDto? FindDetailProduct(Guid id)
		{
			try
			{
				var query = (from q in _productRepository.GetQueryable()
							 join branchTBL in _branchRepository.GetQueryable() on q.BranchId equals branchTBL.Id into branchTB
							 from branch in branchTB.DefaultIfEmpty()
							 join oritbl in _originRepository.GetQueryable() on q.OriginId equals oritbl.Id into originTB
							 from origin in originTB.DefaultIfEmpty()
							 join filetbl in _fileImageRepository.GetQueryable() on q.Id equals filetbl.ProductId into fileTB
							 from file in fileTB.DefaultIfEmpty()
							 where q.Id == id
							 select new ProductDetailDto
							 {
								 ProdcutPrice = q.ProdcutPrice,
								 ProductName = q.ProductName,
								 ProductDescription = q.ProductDescription,
								 ProductMaterial = q.ProductMaterial,
								 BranchName = branch != null ? branch.BranchName : null, // Ensure branch is not null before accessing its properties
								 ProductType = q.ProductType,
								 comment = q.comment,
								 views = q.views,
								 ProductNumber = q.ProductNumber,
								 ProductQuanlity = q.ProductQuanlity,
								 ProductSold = q.ProductSold,
								 OriginName = origin != null ? origin.OriginName : null, // Ensure origin is not null before accessing its properties
								 listFileAndImage = fileTB.Select(fi => new FileImageDto
								 {
									 extension = fi.extension,
									 FilePath = fi.FilePath,
									 mime = fi.mime,
									 CreateAt = fi.CreatedDate
								 }).ToList()
							 }).FirstOrDefault();

				return query;
			}
			catch (Exception)
			{
				return null;
			}
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
