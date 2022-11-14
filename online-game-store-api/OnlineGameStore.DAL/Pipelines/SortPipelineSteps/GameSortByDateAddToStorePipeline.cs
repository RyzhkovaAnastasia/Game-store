using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Pipelines.SortPipelineSteps
{
    public class GameSortByDateAddToStorePipeline : IPipelineStep<List<Game>, List<Game>>
    {
        private readonly bool _isAscending;
        public GameSortByDateAddToStorePipeline(bool isAscending = true)
        {
            _isAscending = isAscending;
        }
        public List<Game> Process(List<Game> input)
        {
            if (_isAscending)
            {
                return input.OrderBy(x => x.Created).ToList();
            }
            return input.OrderByDescending(x => x.Created).ToList();
        }
    }
}

