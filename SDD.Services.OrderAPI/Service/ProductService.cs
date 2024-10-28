using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;
using SDD.Services.OrderAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/Products/{id}");
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
            if (resp!.IsSuccess && resp.Result != null)
            {
                var productDto = JsonConvert.DeserializeObject<ProductDto>(resp.Result.ToString()!);
                if (productDto != null)
                {
                    return productDto;
                }
            }

            // Return an empty ProductDto if the product was not found or if deserialization fails
            return new ProductDto();
        }
    }
}
