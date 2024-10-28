using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SDD.Services.ProductAPI.Contracts;
using SDD.Services.ProductAPI.Models;
using SDD.Services.ProductAPI.Models.Dto;

namespace SDD.Services.ProductAPI.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductApiController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;

        }

        [HttpGet]
        [Authorize]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<ResponseDto>> GetAll([FromQuery] int? skip = null, [FromQuery] int? take = null)
        {
                int skipValue = skip ?? 0;
                int takeValue = take ?? 10;
                return Ok(await _productRepository.GetAllAsync(skipValue, takeValue));
        }

        [HttpGet("{id}")]
        [Authorize]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            return Ok(await _productRepository.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)] // When the product is created successfully.
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // In case of a bad request.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // When the user is not authenticated.
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseDto>> Post([FromBody] CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            return Ok(await _productRepository.AddAsync(product));
        }


        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)] // When the product is created successfully.
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // In case of a bad request.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // When the user is not authenticated.
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseDto>> Put([FromBody] ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            return Ok(await _productRepository.UpdateAsync(product));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)] // When the product is created successfully.
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // In case of a bad request.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // When the user is not authenticated.
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseDto>> Delete(int id)
        {
           return Ok(await _productRepository.DeleteAsync(id));
        }
    }
}
