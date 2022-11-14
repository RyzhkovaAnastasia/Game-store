using AutoMapper;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class GenreMapper : Profile
    {
        public GenreMapper()
        {
            CreateMap<GenreModel, Genre>()
                .ForMember(x => x.ChildGenres, opt => opt.MapFrom(x => x.Children));

            CreateMap<Genre, GenreModel>()
                .ForMember(x => x.Children, opt => opt.MapFrom(x => x.ChildGenres));
        }
    }
}
