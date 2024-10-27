using AutoMapper;
using SDD.Services.ProductAPI.Models;
using SDD.Services.ProductAPI.Models.Dto;

namespace SDD.Services.ProductAPI
{
    public static class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
                config.CreateMap<CreateProductDto, Product>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
