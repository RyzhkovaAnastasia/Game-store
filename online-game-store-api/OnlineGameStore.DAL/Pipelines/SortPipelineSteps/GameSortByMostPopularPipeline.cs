using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Pipelines.SortPipelineSteps
{
    public class GameSortByMostPopularPipeline : IPipelineStep<List<Game>, List<Game>>
    {
        private readonly bool _isAscending;
        public GameSortByMostPopularPipeline(bool isAscending = true)
        {
            _isAscending = isAscending;
        }
        public List<Game> Process(List<Game> input)
        {
            if (_isAscending)
            {
                return input.OrderBy(x => x.ViewsNumber).ToList();
            }
            return input.OrderByDescending(x => x.ViewsNumber).ToList();

        }
    }
}
