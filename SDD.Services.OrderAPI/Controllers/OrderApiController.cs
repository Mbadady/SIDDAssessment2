using AutoMapper;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDD.Services.OrderAPI.Contracts.Repositories;
using SDD.Services.OrderAPI.Models;
using SDD.Services.OrderAPI.Models.Dto;

namespace SDD.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        protected ResponseDto _response;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;
        public OrderApiController(IProductService productService, IMapper mapper, IConfiguration configuration, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            this._response = new ResponseDto();
            _productService = productService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("getorders")]
        public async Task<ActionResult<ResponseDto>> Get(CancellationToken cancellationToken)
        {

            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync(cancellationToken);
                if(orders.Any())
                {
                    _response.Result = orders;
                    _response.Message = "Orders retrieved successfully.";
                    return Ok(_response);
                }
                _response.Result = orders;
                _response.Message = "No order found";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }

        }

        [Authorize]
        [HttpGet("getorder/{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id, cancellationToken);
                if (order == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Order with ID {id} not found.";
                    return NotFound(_response);
                }

                _response.Result = order;
                _response.Message = "Order retrieved successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [Authorize]
        [HttpPost("createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto, CancellationToken cancellationToken)
        {
            try
            {
                // Validate product existence via ProductApi
                var product = await _productService.GetProductById(orderDto.ProductId);
                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Product with ID {orderDto.ProductId} not found.";
                    return NotFound(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid order data.";
                    return BadRequest(_response);
                }

                var order = _mapper.Map<Order>(orderDto);

                var createdOrderDto = await _orderRepository.CreateOrderAsync(order, cancellationToken);
                _response.Result = createdOrderDto;
                _response.Message = "Order created successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderDto orderDto, CancellationToken cancellationToken)
        {
            try
            {
                var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken);
                if (existingOrder == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Order with ID {orderId} not found.";
                    return NotFound(_response);
                }

                // Validate product existence via ProductApi
                var product = await _productService.GetProductById(orderDto.ProductId);
                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Product with ID {orderDto.ProductId} not found.";
                    return NotFound(_response);
                }
                var updatedOrder = _mapper.Map<Order>(orderDto);
                var updatedOrderDto = await _orderRepository.UpdateOrderAsync(updatedOrder, cancellationToken);
                _response.Result = updatedOrderDto;
                _response.Message = "Order updated successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                var orderExists = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken);
                if (orderExists == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Order with ID {orderId} not found.";
                    return NotFound(_response);
                }

                var result = await _orderRepository.DeleteOrderAsync(orderId, cancellationToken);
                if (!result)
                {
                    _response.IsSuccess = false;
                    _response.Message = "An error occurred while deleting the order.";
                    return StatusCode(500, _response);
                }

                _response.Message = $"Order with ID {orderId} has been deleted successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response);
            }
        }
    }
}
