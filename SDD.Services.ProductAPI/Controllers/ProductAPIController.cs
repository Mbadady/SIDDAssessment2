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
        private readonly IProductRepository<Product> _productRepository;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;

        public ProductApiController(IProductRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _response = new ResponseDto();

        }

        [HttpGet]
        [Authorize]
        [EnableRateLimiting("fixed")]
        public async Task<ResponseDto> GetAll([FromQuery] int? skip = null, [FromQuery] int? take = null)
        {
            try
            {
                int skipValue = skip ?? 0;
                int takeValue = take ?? 10;
                var products = await _productRepository.GetAllAsync(skipValue, takeValue);
                _response.Message = "Product found successfully";
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("{id}")]
        [Authorize]
        [EnableRateLimiting("fixed")]
        public async Task<ResponseDto> GetById(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product != null)
                {
                    _response.Result = _mapper.Map<ProductDto>(product);
                    _response.Message = "Product found successfully";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Product not found";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)] // When the product is created successfully.
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // In case of a bad request.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // When the user is not authenticated.
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ResponseDto> Post([FromBody] CreateProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                await _productRepository.AddAsync(product);
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.Message = "Product Created successfully";

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)] // When the product is created successfully.
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // In case of a bad request.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // When the user is not authenticated.
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ResponseDto> Put([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                await _productRepository.UpdateAsync(product);
                _response.Message = "Product updated successfully";
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)] // When the product is created successfully.
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // In case of a bad request.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // When the user is not authenticated.
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                var isDeleted = await _productRepository.DeleteAsync(id);
                _response.IsSuccess = isDeleted;
                _response.Message = isDeleted ? "Deleted successfully" : "Product not found";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
