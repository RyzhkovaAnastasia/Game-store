using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Pipelines.PipelineSteps
{
    public class GameMinPricePipeline : IPipelineStep<List<Game>, List<Game>>
    {
        private readonly decimal? _price;

        public GameMinPricePipeline(decimal? price)
        {
            if (price != null && price < 0)
            {
                price = 0;
            }
            _price = price;
        }

        public List<Game> Process(List<Game> input)
        {
            if (_price != null)
            {
                return input.Where(x => x.Price - (x.Price * ((decimal)x.Discount / 100)) >= _price).ToList();
            }
            return input;
        }
    }
}
