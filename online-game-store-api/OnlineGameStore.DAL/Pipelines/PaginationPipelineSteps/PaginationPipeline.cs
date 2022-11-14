using OnlineGameStore.DAL.DTOs;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Pipelines.PaginationPipelineSteps
{
    public class PaginationPipeline : IPipelineStep<List<Game>, IEnumerable<Game>>
    {
        private readonly GamesPagination _pagination;

        public PaginationPipeline(GamesPagination gamesPagination)
        {
            if (gamesPagination != null)
            {
                _pagination = gamesPagination;
            }
        }

        public IEnumerable<Game> Process(List<Game> input)
        {
            if (_pagination != null)
            {
                if (input.Any())
                {
                    int totalItems = input.Count;
                    int totalPages = (int)Math.Ceiling(totalItems / (decimal)_pagination.ItemsPerPage);

                    if (_pagination.CurrentPage < 1)
                    {
                        _pagination.CurrentPage = 1;
                    }
                    else if (_pagination.CurrentPage > totalPages)
                    {
                        _pagination.CurrentPage = totalPages;
                    }

                    int gamesBeforeCurrentPage = (_pagination.ItemsPerPage * (_pagination.CurrentPage - 1));
                    int gamesOnPage = totalItems - _pagination.ItemsPerPage * _pagination.CurrentPage >= 0 ?
                        _pagination.ItemsPerPage :
                        totalItems - _pagination.ItemsPerPage * (_pagination.CurrentPage - 1);

                    return input.GetRange(gamesBeforeCurrentPage, gamesOnPage);
                }
            }
            return input;
        }
    }
}
