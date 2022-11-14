using OnlineGameStore.Common.Enums;
using OnlineGameStore.DAL.DTOs;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.PaginationPipelineSteps;
using OnlineGameStore.DAL.Pipelines.PipelineSteps;
using OnlineGameStore.DAL.Pipelines.SortPipelineSteps;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Pipelines.Pipelines
{
    public class GamePipeline : GamePipeline<List<Game>, List<Game>>
    {
        public GamePipeline(GamesFilterComponents gameFilter)
        {
            if (gameFilter != null)
            {
                PipelineSteps = input => input
                #region Filtering
                .Step(new GameMaxPricePipeline(gameFilter.Filter?.MaxPrice))
                    .Step(new GameMinPricePipeline(gameFilter.Filter?.MinPrice))
                    .Step(new GameNamePipeline(gameFilter.Filter?.Name))
                    .Step(new GamePublishedPipeline(gameFilter.Filter?.Published ?? GamePublishedDate.allTime))
                    .Step(new GameGenresPipeline(gameFilter.Filter?.Genres))
                    .Step(new GamePlatformsPipeline(gameFilter.Filter?.Platforms))
                    .Step(new GamePublishersPipeline(gameFilter.Filter?.Publishers))
                #endregion
                #region Sorting
                .Step(SelectSortMethod(gameFilter.Sort?.GameSortMethod, gameFilter.Sort.IsAscending));
                #endregion
            }
            else
            {
                PipelineSteps = input => input;
            }
        }

        public IPipelineStep<List<Game>, List<Game>> SelectSortMethod(GameSortMethod? gameSortField, bool isAscending = true)
        {
            return gameSortField switch
            {
                GameSortMethod.MostPopular => new GameSortByMostPopularPipeline(isAscending),
                GameSortMethod.MostCommented => new GameSortByCommentNumberPipeline(isAscending),
                GameSortMethod.Price => new GameSortByPricePipeline(isAscending),
                GameSortMethod.NewAdded => new GameSortByDateAddToStorePipeline(isAscending),
                _ => new GameSortSkipPipeline()
            };
        }
    }
}