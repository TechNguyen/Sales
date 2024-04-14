using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sale.Domain.Core;
using Sale.Domain.Entities;
using Sale.Service.Common;
using Sale.Service.Constant;
using Sale.Service.Dtos;
using Sale.Service.Dtos.FileImageDto;
using Sale.Service.Dtos.ProductDto;
using Sale.Service.FileImageService;
using Sale.Service.ProductService;
using Sales.Model.Product;
using System.Net.WebSockets;
using System.Security.Claims;

namespace Sales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
		private readonly IFileImageService _fileImageService;
        public ProductController(
			ILogger<ProductController> logger,
            IProductService productService,
            IMapper mapper,
			IFileImageService fileImageService
			) { 
            _productService = productService;
            _logger = logger;
            _mapper = mapper;
			_fileImageService = fileImageService;
        }

        /// <summary>
        /// thêm mới sản phẩm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

		[HttpPost("create")]
		[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreateVM entity)
        {
            try
            {
                var obj = new Product();
                obj = _mapper.Map<Product>(entity);
				await _productService.Create(obj);
				//xử lý upload
				foreach (var img in entity.ListFileImg)
				{
					var rs = FileExtension.UploadFile(img);
					if (rs != null)
					{
						var filenew = new FileImageDto()
						{
							ProductId = obj.Id,
							CreateAt = DateTime.Now,
							extension = rs[FileConstant.FILEDETAIL][FileConstant.FILEXTEMSION],
							fileSize = rs[FileConstant.FILESIZE],
							FileName = rs[FileConstant.FILEDETAIL][FileConstant.FILENAMECONVERT],
							FilePath = rs[FileConstant.FILEPATH],
							mime = rs[FileConstant.MIME],
						};
						var obbjFile = new FileImage();
						obbjFile = _mapper.Map<FileImage>(filenew);
						obbjFile.CreatedDate = DateTime.Now;
						await _fileImageService.Create(obbjFile);
					}
				}
				return StatusCode(StatusCodes.Status201Created, new ResponseWithDataDto<dynamic>
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



        [HttpPost("getall")]
		[AllowAnonymous]
		public async Task<IActionResult> GetDataByPage([FromBody] ProductSearchDto searchEntity)
        {
            try
            {
                var obj = await _productService.GetDataByPage(searchEntity); 
				return StatusCode(StatusCodes.Status200OK, new ResponseWithDataDto<PageList<ProductDto>?>
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
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> Edit([FromBody] EditVM entity, [FromQuery] Guid id)
		{
			try
			{
				var data = _productService.GetById(id);
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
					data = _mapper.Map<EditVM, Product>(entity, data);
					await _productService.Update(data);
					return StatusCode(StatusCodes.Status200OK, new ResponseWithDataDto<Product>
					{
						Data = data,
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
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete([FromQuery] Guid Id)
        {

            try
            {
                var data = _productService.FindBy(x => x.Id == Id && (x.IsDelete == false || x.IsDelete == null)).FirstOrDefault();
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
					//Xóa hết ở trong File
					var listFile = _fileImageService.FindBy(x => x.ProductId == Id).ToList();
					await _fileImageService.Delete(listFile);
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
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteArange([FromBody] List<Guid> ListId)
		{
			try
			{
				foreach (var item in ListId)
                {
					var data = _productService.GetById(item);
                    if(data != null)
                    {
						var listFile = _fileImageService.FindBy(x => x.ProductId == item).ToList();
						await _fileImageService.Delete(listFile);
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
		[AllowAnonymous]
		public async Task<IActionResult> GetById([FromQuery] Guid id)
		{
			try
			{
				var data = _productService.FindDetailProduct(id);
				return StatusCode(StatusCodes.Status200OK, new ResponseWithDataDto<ProductDetailDto>
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



		[HttpPost("delete-soft")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteSoft([FromQuery] Guid id)
		{

			try
			{
				var deletePr = _productService.FindBy(x => x.Id == id).FirstOrDefault();
			
				if (deletePr == null)
				{
					return StatusCode(StatusCodes.Status400BadRequest, new ResponseWithMessageDto
					{
						Message = "Không tồn tại",
						Status = StatusConstant.ERROR
					});
				}
				else
				{
					deletePr.IsDelete = true;
					deletePr.DeleteTime = DateTime.Now;
					var claims = HttpContext.User.Identity as ClaimsIdentity;
					if (claims != null)
					{
						var name = claims.FindFirst(ClaimTypes.Name);
						deletePr.DeleteBy = name.Value;
					}
					await _productService.Update(deletePr);
					return StatusCode(StatusCodes.Status200OK, new ResponseWithDataDto<dynamic>
					{
						Data = deletePr,
						Message = "Xóa mềm thành công",
						Status = StatusConstant.SUCCESS
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

	}
}
