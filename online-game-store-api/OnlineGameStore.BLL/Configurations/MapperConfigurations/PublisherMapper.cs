using AutoMapper;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class PublisherMapper : Profile
    {
        public PublisherMapper()
        {
            CreateMap<Publisher, PublisherModel>()
                .ForMember(x => x.HomePage, opt => opt.MapFrom(x => x.HomePage != "NULL" ? x.HomePage : null))
                .ReverseMap();
        }
    }
}
