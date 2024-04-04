using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sale.Domain.Entities;
using Sale.Service.Constant;
using Sale.Service.Dtos;
using Sale.Service.Dtos.ProductDto;
using Sale.Service.ProductService;
using Sales.Model.Product;
using System.Net.WebSockets;

namespace Sales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductController(
			ILogger<ProductController> logger,
            IProductService productService,
            IMapper mapper
			) { 
            _productService = productService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// thêm mới sản phẩm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

		[HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateVM entity)
        {
            try
            {
                var obj = new Product();
                obj = _mapper.Map<Product>(entity);
                await _productService.Create(obj);
                return StatusCode(StatusCodes.Status201Created, new ResponseWithDataDto<Product>
                {
                    Data = obj,
                    Status = StatusConstant.SUCCESS,
					Message = "Thêm mới sản phẩm thành công"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto
				{
					Status = StatusConstant.ERROR,
					Message = ex.Message
				});
            }
        }

    }
}
