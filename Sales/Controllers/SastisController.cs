using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sale.Service.Constant;
using Sale.Service.Dtos;

namespace Sales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SastisController : ControllerBase
	{

		public SastisController() { }



		[HttpGet]
		public async Task<IActionResult> DashboardInformation()
		{
			try
			{
				return StatusCode(StatusCodes.Status200OK, new 
				{
					Message = "Thong ke thanh cong",
					
				});

			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto
				{
					Message = ex.Message,
					Status = StatusConstant.ERROR
				});
			}
		}
	}

}
