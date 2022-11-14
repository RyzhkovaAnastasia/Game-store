using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Pipelines.SortPipelineSteps
{
    public class GameSortByPricePipeline : IPipelineStep<List<Game>, List<Game>>
    {
        private readonly bool _isAscending;
        public GameSortByPricePipeline(bool isAscending = true)
        {
            _isAscending = isAscending;
        }
        public List<Game> Process(List<Game> input)
        {
            if (_isAscending)
            {
                return input.OrderBy(x => x.Price - (x.Price * ((decimal)x.Discount / 100))).ToList();
            }
            return input.OrderByDescending(x => x.Price - (x.Price * ((decimal)x.Discount / 100))).ToList();
        }
    }
}
