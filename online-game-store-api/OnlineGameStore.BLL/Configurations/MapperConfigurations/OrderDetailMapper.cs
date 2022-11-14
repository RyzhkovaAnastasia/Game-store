using AutoMapper;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class OrderDetailMapper : Profile
    {
        public OrderDetailMapper()
        {
            CreateMap<OrderDetail, OrderDetailModel>();
            //.ForMember(x => x.GameId,
            //opt => opt.MapFrom(o => o.GameId != null && o.GameId != Guid.Empty ?
            //  o.GameId :
            //  GuidIntConverter.IntToGuid(o.MongoGameId.HasValue ? o.MongoGameId : null)));

            CreateMap<OrderDetailModel, OrderDetail>()
                //.ForMember(x => x.MongoGameId, opt => opt.MapFrom(o =>
                //   GuidIntConverter.GuidToInt(o.GameId)))
                .ForMember(x => x.GameId, opt => opt.MapFrom(o =>
                   o.GameId))
                .AfterMap((src, dest) => dest.Game = null);
        }
    }
}
