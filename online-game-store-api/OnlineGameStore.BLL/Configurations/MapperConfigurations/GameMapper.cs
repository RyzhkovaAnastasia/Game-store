using AutoMapper;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.DTOs;
using OnlineGameStore.DAL.Entities;
using System;
using System.Linq;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class GameMapper : Profile
    {
        public GameMapper()
        {
            CreateMap<Game, GameModel>()
                 .ForMember(
                game => game.IsDownloaded,
                opt => opt.MapFrom(game => !string.IsNullOrEmpty(game.DownloadPath)
                ))
                  .ForMember(
                game => game.IsCommented,
                opt => opt.MapFrom(game => !string.IsNullOrEmpty(game.DownloadPath)
                ))
                .ForMember(
                game => game.PlatformTypes,
                opt => opt.MapFrom(game =>
                game.PlatformTypes.Select(
                    ptModel => ptModel.PlatformType)
                ))
                .ForMember(gameModel => gameModel.Genres,
                opt => opt.MapFrom(gm =>
                gm.Genres.Select(
                    genreModel => genreModel.Genre)
                ))
                .ForMember(gameModel => gameModel.CommentsNumber,
                opt => opt.MapFrom(game =>
                game.Comments.Count(x => !x.IsDeleted)
                ));

            CreateMap<GameModel, Game>()
                .ForMember(
                game => game.PlatformTypes,
                opt => opt.MapFrom(gameModel => gameModel.PlatformTypes.Select(
                    ptModel =>
                    new GamePlatformType()
                    {
                        GameId = gameModel.Id,
                        PlatformTypeId = ptModel.Id
                    })
                ))
                .ForMember(gpt => gpt.Genres,
                opt => opt.MapFrom(gm => gm.Genres.Select(
                    genreModel => new GameGenre()
                    {
                        GameId = gm.Id,
                        GenreId = genreModel.Id
                    })
                ))
                .ForMember(game => game.Publisher, opt => opt.MapFrom(x => (Publisher)null))
                .ForMember(game => game.PublisherId, opt => opt.MapFrom(x => x.Publisher != null ? x.Publisher.Id : (Guid?)null));

            CreateMap<GamesFilterModel, GamesFilter>();
            CreateMap<GamesPaginationModel, GamesPagination>();
            CreateMap<GameSortModel, GameSort>();
            CreateMap<GamesFilterComponentsModel, GamesFilterComponents>();
        }
    }
}
