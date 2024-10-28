using AutoMapper;
using Azure;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using SDD.Services.OrderAPI.Data;
using SDD.Services.OrderAPI.Models;
using SDD.Services.OrderAPI.Models.Dto;

namespace SDD.Services.OrderAPI.Contracts.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;


        public OrderRepository(AppDbContext context, IMapper mapper, IProductService productService)
        {
            _dbContext = context;
            _mapper = mapper;
            _productService = productService;
        }
        public async Task<ResponseDto> CreateOrderAsync(Order order, CancellationToken cancellation = default)
        {
            try
            {
                var product = await _productService.GetProductById(order.ProductId);
                if (product == null)
                {
                    return new ResponseDto { IsSuccess = true, Message = "Product not found for this order", Result = null };
                }
                await _dbContext.Orders.AddAsync(order, cancellation);
                await _dbContext.SaveChangesAsync(cancellation);
                var orderDto = _mapper.Map<OrderDto>(order);
                return new ResponseDto { IsSuccess = true, Message = "Order created Successfully", Result = orderDto };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = $"An error occurred while creating the order: {ex.Message}", Result = null };
            }
        }

        public async Task<ResponseDto> DeleteOrderAsync(int orderId, CancellationToken cancellation = default)
        {
            try
            {
                var entity = await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellation);
                if (entity == null)
                {
                    return new ResponseDto { IsSuccess = true, Message = "No Order found for this id" };
                }

                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellation);
                return new ResponseDto { IsSuccess = true, Message = "Order deleted successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = true, Message = $"An error occurred while deleting the order: {ex.Message}" };

            }
        }

        public async Task<ResponseDto> GetAllOrdersAsync(int skip, int take, CancellationToken cancellation = default)
        {
            try
            {
                var query = _dbContext.Orders.AsNoTracking();
                var count = await query.CountAsync(cancellation);
                if (count == 0)
                {
                    return new ResponseDto { IsSuccess = true, Message = "No order found", Result = new List<OrderDto>() };
                }
                var orders = await query
                                                       .Skip(skip)
                                                       .Take(take)
                                                       .ToListAsync(cancellation);
                var orderDto = _mapper.Map<List<OrderDto>>(orders);

                return new ResponseDto { IsSuccess = true, Message = $"{count} Orders Fetched Successfully", Result = new { count, orderDto } };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = $"An error occurred while getting the orders: {ex.Message}" };
            }
        }

        public async Task<ResponseDto> GetOrderByIdAsync(int orderId, CancellationToken cancellation = default)
        {
            try
            {
                var order = await _dbContext.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == orderId, cancellation);
                if (order == null)
                {
                    return new ResponseDto { IsSuccess = true, Message = "No Order found for this id" };
                }
                var orderDto = _mapper.Map<OrderDto>(order);

                return new ResponseDto { IsSuccess = true, Message = "Order found successfully", Result = orderDto };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = true, Message = $"An error occurred while getting the order: {ex.Message}", Result = null };

            }

        }

        public async Task<ResponseDto> UpdateOrderAsync(Order order, CancellationToken cancellation = default)
        {
            try
            {
                // Check if the product exists in the database
                var existingOrder = await _dbContext.Orders.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.OrderId == order.OrderId, cancellation);

                if (existingOrder == null)
                {
                    return new ResponseDto { IsSuccess = false, Message = "No order found for this id" };
                }

                var product = await _productService.GetProductById(order.ProductId);
                if (product == null)
                {
                    return new ResponseDto { IsSuccess = true, Message = "Product not found for this order", Result = null };
                }
                // Update the product directly, since the 'product' object contains updated values
                _dbContext.Orders.Update(order);
                // Save changes to the database
                await _dbContext.SaveChangesAsync(cancellation);
                var orderDto = _mapper.Map<OrderDto>(order);

                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = "Order updated successfully",
                    Result = orderDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false, // Marking it as failure for proper error indication
                    Message = $"An error occurred while updating the order: {ex.Message}",
                    Result = null
                };
            }
        }
    }
}
