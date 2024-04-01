using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{

		[HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}
