using SDD.Services.ProductAPI.Models;
using SDD.Services.ProductAPI.Models.Dto;

namespace SDD.Services.ProductAPI.Contracts
{
    public interface IProductRepository
    {
        Task<ResponseDto> GetByIdAsync(int id, CancellationToken cancellation = default);
        Task<ResponseDto> AddAsync(Product product, CancellationToken cancellation = default);
        Task<ResponseDto> UpdateAsync(Product product, CancellationToken cancellation = default);
        Task<ResponseDto> DeleteAsync(int id, CancellationToken cancellation = default);
        Task<ResponseDto> GetAllAsync(int skip, int take, CancellationToken cancellation = default);
    }
}
