using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sale.Domain.Entities;
using Sale.Service.BranchService;
using Sale.Service.Common;
using Sale.Service.Constant;
using Sale.Service.Dtos;
using Sale.Service.Dtos.BranchDto;
using Sale.Service.FileImageService;
using Sales.Model.Branch;
using System.Security.Claims;

namespace Sales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BranchController : ControllerBase
	{
		private readonly IBranchService _branchService;
		private readonly IMapper _mapper;
		private readonly IFileImageService _fileImageService;
        public BranchController(
			IBranchService branchService,
			IMapper mapper,
			IFileImageService fileImageService

			) {
			_branchService = branchService;
			_mapper = mapper;
			_fileImageService = fileImageService;
		}



		[HttpPost("create")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromForm] CreateVM entity)
		{
			try
			{
				var obj = new Branch();
				obj = _mapper.Map<Branch>(entity);
				obj.CreatedDate = DateTime.Now;
				//xử lý upload

				var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
				if (claimsIdentity != null)
				{
					var name = claimsIdentity.FindFirst(ClaimTypes.Name);
					obj.CreatedBy = name.Value;
				}

				
				await _branchService.Create(obj);



				return StatusCode(StatusCodes.Status201Created, new ResponseWithDataDto<Branch>
				{
					Data = obj,
					Status = StatusConstant.SUCCESS,
					Message = "Thêm mới thương hiệu thành công"
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
		public async Task<IActionResult> GetDataByPage([FromForm] BranchSearchDto searchEntity)
		{

			try
			{
				var obj = await _branchService.GetDataByPage(searchEntity);
				return StatusCode(StatusCodes.Status201Created, new ResponseWithDataDto<PageList<BranchDto>?>
				{
					Data = obj,
					Status = StatusConstant.SUCCESS,
					Message = "Lấy thương hiệu thành công"
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

		public async Task<IActionResult> Edit([FromForm] EditVM entity)
		{

			try
			{
				var data = _branchService.GetById(entity.id);
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
					var obj = _mapper.Map<Branch>(entity);
					await _branchService.Update(obj);
					return StatusCode(StatusCodes.Status200OK, new ResponseWithDataDto<Branch>
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
				var data = _branchService.GetById(Id);
				if (data == null)
				{
					return StatusCode(StatusCodes.Status400BadRequest, new ResponseWithMessageDto
					{
						Status = StatusConstant.ERROR,
						Message = "Không tồn tại thương hi"
					});
				}
				else
				{
					await _branchService.Delete(data);
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
					var data = _branchService.GetById(item);
					if (data != null)
					{
						await _branchService.Delete(data);
					}
				}
				return StatusCode(StatusCodes.Status200OK, new ResponseWithMessageDto
				{
					Status = StatusConstant.SUCCESS,
					Message = "Xóa thương hiệu thành công"
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
