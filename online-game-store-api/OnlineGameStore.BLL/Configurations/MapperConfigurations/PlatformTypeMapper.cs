using AutoMapper;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class PlatformTypeMapper : Profile
    {
        public PlatformTypeMapper()
        {
            CreateMap<PlatformType, PlatformTypeModel>().ReverseMap();
        }
    }
}
