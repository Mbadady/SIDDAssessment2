using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SDD.Services.OrderAPI.Data;
using SDD.Services.OrderAPI.Models;
using SDD.Services.OrderAPI.Models.Dto;

namespace SDD.Services.OrderAPI.Contracts.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OrderDto> CreateOrderAsync(Order order, CancellationToken cancellation = default)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellation);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<bool> DeleteOrderAsync(int orderId, CancellationToken cancellation = default)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellation);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync(cancellation);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(CancellationToken cancellation = default)
        {
            var orders = await _context.Orders.AsNoTracking().ToListAsync(cancellation);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId, CancellationToken cancellation = default)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellation);

            return _mapper.Map<OrderDto>(order);

        }

        public async Task<OrderDto> UpdateOrderAsync(Order order, CancellationToken cancellation = default)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellation);
            return _mapper.Map<OrderDto>(order);
        }
    }
}
