using AutoMapper;
using SDD.Services.OrderAPI.Models;
using SDD.Services.OrderAPI.Models.Dto;

namespace Mango.Services.OrderAPI
{
    public static class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {

                config.CreateMap<OrderDto, Order>().ReverseMap();
                config.CreateMap<CreateOrderDto, Order>().ReverseMap();

            });
            return mappingConfig;
        }
    }
}
