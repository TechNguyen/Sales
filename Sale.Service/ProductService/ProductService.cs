using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using Sale.Domain;
using Sale.Domain.Entities;
using Sale.Repository.BranchRepository;
using Sale.Repository.FileImageRepository;
using Sale.Repository.OriginRepository;
using Sale.Repository.ProductRepository;
using Sale.Service.Common;
using Sale.Service.Constant;
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
								 ProductQuanlity = q.ProductQuanlity,
								 ProductSold = q.ProductSold,
								 OriginName = origin != null ? origin.OriginName : null, // Ensure origin is not null before accessing its properties
							 }).FirstOrDefault();


				var queryImg = (from fileImg in _fileImageRepository.GetQueryable()
								select new FileImageDto
								{
									extension = fileImg.extension,
									fileSize = fileImg.fileSize,
									FileName = fileImg.FileName,
									FilePath = fileImg.FilePath,
									mime = fileImg.mime,
									CreateAt = fileImg.CreatedDate,
								}).ToList();
				query.listFileAndImage = new List<FileImageDto>();
				query.listFileAndImage.AddRange(queryImg);
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


				var query = (from q in _productRepository.GetQueryable()
							 join btbl in _branchRepository.GetQueryable() on q.BranchId equals btbl.Id into bt
							 from b in bt.DefaultIfEmpty()
							 join otbl in _originRepository.GetQueryable() on q.OriginId equals otbl.Id into ot
							 from o in ot.DefaultIfEmpty()
							 where q.IsDelete == false || q.IsDelete == null
							 select new
							 {
								 Product = q,
								 Branch = b,
								 Origin = o,
								 Files = (from file in _fileImageRepository.GetQueryable()
										  where file.ProductId == q.Id
										  select new FileImageDto
										  {
											  extension = file.extension,
											  FileName = file.FileName,
											  FilePath = file.FilePath, 
											  fileSize = file.fileSize,
											  CreateAt= file.CreatedDate,
											  mime = file.mime
										  }).ToList()
							 })
							.AsEnumerable()
							.Select(x => new ProductDto
							{
								ProdcutPrice = x.Product.ProdcutPrice,
								ProductName = x.Product.ProductName,
								ProductDescription = x.Product.ProductDescription,
								ProductMaterial = x.Product.ProductMaterial,
								BranchName = x.Branch?.BranchName,
								ProductType = x.Product.ProductType,
								comment = x.Product.comment,
								views = x.Product.views,
								OriginName = x.Origin?.OriginName,
								ProductQuanlity = x.Product.ProductQuanlity,
								ProductSold = x.Product.ProductSold,
								Id = x.Product.Id,
								rate = x.Product.rate,
								listFile = x.Files
							}).AsQueryable();


				

				if(searchDto != null)
				{
					if(!string.IsNullOrEmpty(searchDto.ProductName))
					{
						query = query.Where(delegate (ProductDto b)
						{
							return b.ProductName.RemoveAccentsUnicode().ToLower().Contains(searchDto.ProductName.RemoveAccentsUnicode().ToLower());
						}).AsQueryable();
					}
					if(!string.IsNullOrEmpty(searchDto.BranchId.ToString()))
					{
						query = query.Where(x => x.BranchId == searchDto.BranchId);	
					}
					if(!string.IsNullOrEmpty(searchDto.OriginId.ToString()))
					{
						query = query.Where(x => x.OriginId == searchDto.OriginId);	
					}
					if(searchDto.PageIndex == null || searchDto.PageIndex <= 0)
					{
						searchDto.PageIndex = 1;
					}
					if (searchDto.PageSize == null || searchDto.PageSize <= 0)
					{
						searchDto.PageSize = 1;
					}
					if(searchDto.listPrice != null)
					{
                        foreach (var item in searchDto.listPrice)
						{
							var minPrice = item[PriceConstant.MIN];
							var maxPrice = item[PriceConstant.MAX];
							query = query.Where(x => x.ProdcutPrice >= minPrice && x.ProdcutPrice <= maxPrice);
						}
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
