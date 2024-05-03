using Microsoft.EntityFrameworkCore;
using Sale.Domain;
using Sale.Repository.BranchRepository;
using Sale.Repository.OrdersRepository;
using Sale.Repository.OriginRepository;
using Sale.Repository.ProductRepository;
using Sale.Service.Constant;
using Sale.Service.Dtos.OrdersDto;
using Sale.Service.OrdersService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Service.SastisService
{
	public class SastisService : ISastisService
	{

		private readonly SaleContext context;
		private readonly IOrdersService _service;
		private readonly IProductRepository _productRepository;
		private readonly IBranchRepository _branchRepository;
		private readonly IOrdersRepository _ordersRepository;
		private readonly IOriginRepository _originRepository;
		public SastisService(SaleContext saleContext)
		{
			context = saleContext;
		}

		public dynamic GetAllData(int? year = null,int? months = null, int? weeeks = null,int? days = null)
		{
			var sastis = new SastisDto();
			var querrycountProducts = context.Products.AsQueryable();
			sastis.dataCountBranchs = context.Branchs.Count();
			sastis.dataCountOrigins = context.Origin.Count();
			var dataCountOrders = context.Orders.AsQueryable();
			if (year != null && year > 0)
			{
				sastis.dataProduct = querrycountProducts.Where(x => x.CreatedDate.Year == year).Count();
				sastis.orderSuccess = dataCountOrders.Where(x => x.Status == OrdersConstant.THANHCONG && x.CreatedDate.Year == year).Count();
				sastis.orderFailed = dataCountOrders.Where(x => x.Status == OrdersConstant.THATBAI && x.CreatedDate.Year == year).Count();
				sastis.orderCancle = dataCountOrders.Where(x => x.Status == OrdersConstant.DAHUY && x.CreatedDate.Year == year).Count();
			}
			else if(months != null && months > 0)
			{
				sastis.dataProduct = querrycountProducts.Where(x => x.CreatedDate.Month == months && x.CreatedDate.Year == year).Count();
			}
            return sastis;
			
		}

	}
}
