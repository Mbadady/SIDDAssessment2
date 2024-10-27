using SDD.Services.OrderAPI.Models;
using SDD.Services.OrderAPI.Models.Dto;

namespace SDD.Services.OrderAPI.Contracts.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync(CancellationToken cancellation = default);
        Task<OrderDto> CreateOrderAsync(Order order, CancellationToken cancellation = default);
        Task<OrderDto> GetOrderByIdAsync(int orderId, CancellationToken cancellation = default);
        Task<OrderDto> UpdateOrderAsync(Order order, CancellationToken cancellation = default);
        Task<bool> DeleteOrderAsync(int orderId, CancellationToken cancellation = default);
    }
}
