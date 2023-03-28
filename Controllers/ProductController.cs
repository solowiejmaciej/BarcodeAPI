using BarcodeAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BarcodeAPI.Services;

namespace BarcodeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/barcode")]
    public class Product : ControllerBase
    {
        private readonly IProductService _productService;

        public Product(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{EAN}")]
        public async Task<ActionResult<OpenFoodApiProductRequestModel>> GetFromAPI([FromRoute] string EAN)
        {
            var productFromApi = await _productService.GetProduct(EAN.Trim());

            return Ok(productFromApi);
        }
    }
}