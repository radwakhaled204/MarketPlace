using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Product.Api.Domain.Interfaces;
using Product.Api.Application.Dtos;

namespace Product.Api.Api.Controllers
{
    [ApiController]
    //[ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ICrmProductClient _products;

        public ProductController(ICrmProductClient products)
        {
            _products = products;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll(CancellationToken ct)
            => Ok(await _products.GetAllAsync(ct));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken ct)
        {
            var product = await _products.GetByIdAsync(id, ct);
            return product is null ? NotFound() : Ok(product);
        }
    }
}



