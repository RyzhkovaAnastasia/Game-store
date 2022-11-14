using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Pipelines.PipelineSteps
{
    public class GamePublishersPipeline : IPipelineStep<List<Game>, List<Game>>
    {
        private readonly IEnumerable<Publisher> _publishers;
        public GamePublishersPipeline(IEnumerable<Publisher> publishers)
        {
            _publishers = publishers;
        }
        public List<Game> Process(List<Game> input)
        {
            if (_publishers != null && _publishers.Any())
            {
                return input.Where(x => _publishers.Select(x => x.Id).Contains(x.PublisherId.GetValueOrDefault())).ToList();
            }
            return input;
        }
    }
}
