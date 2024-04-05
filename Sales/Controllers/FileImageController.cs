using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sale.Service.Constant;
using Sale.Service.Dtos;
using Sale.Service.Dtos.FileImageDto;
using Sale.Service.FileImageService;
using Sale.Service.ProductService;

namespace Sales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FileImageController : ControllerBase
	{


        private readonly IFileImageService _fileImageService;
        public  FileImageController(IFileImageService fileImageService)
        {
           _fileImageService = fileImageService;    
        }

        /// <summary>
        /// get fiel and image by productID
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
		[HttpGet("Get-By-Id")]
        public async Task<IActionResult> GetFileById([FromBody] Guid productId)
        {
            try 
            {
                var fileImage = _fileImageService.FindByProductID(productId);
                if(fileImage != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseWithDataDto<List<FileImageDto>>
                    {
                        Data = fileImage,
                        Status = StatusConstant.SUCCESS,
                        Message = "Lấy thông tin thành công"
                    });
                }
                else
                {
					return StatusCode(StatusCodes.Status200OK, new ResponseWithMessageDto
					{
						Status = StatusConstant.SUCCESS,
						Message = "Lấy thông tin thành công"
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
