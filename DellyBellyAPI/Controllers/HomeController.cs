using DellyBelly.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("bestsellers")]
        public async Task<IActionResult> GetBestSellers()
        {
            var products = await _productService.GetBestSellersAsync(10); // Default to 10
            return Ok(products);
        }

        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommended()
        {
            var products = await _productService.GetRecommendedAsync(10); // Default to 10
            return Ok(products);
        }
    }
}
