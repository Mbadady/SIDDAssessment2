using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        public OrderApiController(IMapper mapper, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("getorders")]
        public async Task<ActionResult<ResponseDto>> Get([FromQuery] int? skip = null, [FromQuery] int? take = null)
        {

            int skipValue = skip ?? 0;
            int takeValue = take ?? 10;
            return Ok(await _orderRepository.GetAllOrdersAsync(skipValue, takeValue));

        }

        [Authorize]
        [HttpGet("getorder/{id}")]
        public async Task<ActionResult<ResponseDto>> Get(int id)
        {
            return Ok(await _orderRepository.GetOrderByIdAsync(id));

        }

        [Authorize]
        [HttpPost("createorder")]
        public async Task<ActionResult<ResponseDto>> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            return Ok(await _orderRepository.CreateOrderAsync(order));
        }

        [HttpPut("updateorder")]
        public async Task<ActionResult<ResponseDto>> UpdateOrder([FromBody] OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            return Ok(await _orderRepository.UpdateOrderAsync(order));
        }

        [HttpDelete("{orderId}")]
        public async Task<ActionResult<ResponseDto>> DeleteOrder(int orderId)
        {
            return Ok(await _orderRepository.DeleteOrderAsync(orderId));
        }
    }
}
