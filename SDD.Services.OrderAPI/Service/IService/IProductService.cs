using SDD.Services.OrderAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<ProductDto> GetProductById(int id);
    }
}
