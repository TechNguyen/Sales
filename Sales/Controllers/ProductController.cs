using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sale.Service.ProductService;
using Sales.Model.ProductModel;
using System.Net.WebSockets;

namespace Sales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        public ProductController(
			ILogger<ProductController> logger,
            IProductService productService
			) { 
            _productService = productService;
            _logger = logger;
        }

		[HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateVM entity)
        {
            try
            {

            
                return StatusCode(StatusCodes.Status201Created, new Response)
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}
