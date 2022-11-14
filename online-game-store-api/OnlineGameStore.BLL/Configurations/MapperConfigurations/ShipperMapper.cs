using AutoMapper;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class ShipperMapper : Profile
    {
        public ShipperMapper()
        {
            CreateMap<Shipper, ShipperModel>().ReverseMap();
        }
    }
}
