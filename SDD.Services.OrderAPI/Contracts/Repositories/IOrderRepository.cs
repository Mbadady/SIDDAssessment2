using SDD.Services.OrderAPI.Models;
using SDD.Services.OrderAPI.Models.Dto;

namespace SDD.Services.OrderAPI.Contracts.Repositories
{
    public interface IOrderRepository
    {
        Task<ResponseDto> GetAllOrdersAsync(int skip, int take, CancellationToken cancellation = default);
        Task<ResponseDto> CreateOrderAsync(Order order, CancellationToken cancellation = default);
        Task<ResponseDto> GetOrderByIdAsync(int orderId, CancellationToken cancellation = default);
        Task<ResponseDto> UpdateOrderAsync(Order order, CancellationToken cancellation = default);
        Task<ResponseDto> DeleteOrderAsync(int orderId, CancellationToken cancellation = default);
    }
}
