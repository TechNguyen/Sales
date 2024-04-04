using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sale.Domain.Core;
using Sale.Domain.Entities;
using Sale.Service.Common;
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
		[Authorize]
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



        [HttpGet("getall")]

		public async Task<IActionResult> GetDataByPage([FromBody] ProductSearchDto searchEntity)
        {

            try
            {
                var obj = await _productService.GetDataByPage(searchEntity); 
				return StatusCode(StatusCodes.Status201Created, new ResponseWithDataDto<PageList<ProductDto>?>
				{
					Data = obj,
					Status = StatusConstant.SUCCESS,
					Message = "Lấy sản phẩm thành công"
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

		/// <summary>
		/// Cập nhật
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPut("Edit")]
		[Authorize]

		public async Task<IActionResult> Edit([FromBody] EditVM entity)
		{
			try
			{
				var data = _productService.GetById(entity.id);
				if (data == null)
				{
					return StatusCode(StatusCodes.Status400BadRequest, new ResponseWithMessageDto
					{
						Status = StatusConstant.ERROR,
						Message = "Không tồn tại sản phẩm"
					});
				}
				else
				{
					var obj = _mapper.Map<Product>(entity);
					await _productService.Update(obj);
					return StatusCode(StatusCodes.Status200OK, new ResponseWithDataDto<Product>
					{
						Data = obj,
						Status = StatusConstant.SUCCESS,
						Message = "Cập nhật sản phẩm thành công"
					});
				}

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



		[HttpDelete("delete")]
		[Authorize]

		public async Task<IActionResult> Delete([FromQuery] Guid Id)
        {

            try
            {
                var data = _productService.GetById(Id);
                if(data == null)
                {
					return StatusCode(StatusCodes.Status400BadRequest, new ResponseWithMessageDto
					{
						Status = StatusConstant.ERROR,
						Message = "Không tồn tại sản phẩm"
					});
				}
                else
                {
					await _productService.Delete(data);
					return StatusCode(StatusCodes.Status200OK, new ResponseWithMessageDto
					{
						Status = StatusConstant.SUCCESS,
						Message = "Xóa sản phẩm thành công"
					});
				}

				
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



		[HttpDelete("delete-arrange")]
		[Authorize]

		public async Task<IActionResult> DeleteArange([FromBody] List<Guid> ListId)
		{

			try
			{
				foreach (var item in ListId)
                {
					var data = _productService.GetById(item);
                    if(data != null)
                    {
					    await _productService.Delete(data);
                    }
				}
				return StatusCode(StatusCodes.Status200OK, new ResponseWithMessageDto
				{
					Status = StatusConstant.SUCCESS,
					Message = "Xóa sản phẩm thành công"
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



		[HttpGet("find-by-id")]
		public async Task<IActionResult> GetById([FromBody] Guid id)
		{

			try
			{
				var data = _productService.GetById(id);
				return StatusCode(StatusCodes.Status200OK, new ResponseWithDataDto<Product>
				{
					Status = StatusConstant.SUCCESS,
					Data = data,
					Message = "Thành công"
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
