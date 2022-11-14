using OnlineGameStore.Common.Enums;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Pipelines.PipelineSteps
{
    public class GamePublishedPipeline : IPipelineStep<List<Game>, List<Game>>
    {
        private readonly GamePublishedDate _timeSpan;
        public GamePublishedPipeline(GamePublishedDate timeSpan = GamePublishedDate.allTime)
        {
            _timeSpan = timeSpan;
        }
        public List<Game> Process(List<Game> input)
        {
            if (_timeSpan == GamePublishedDate.allTime)
            {
                return input;
            }
            return input.Where(x => (DateTime.UtcNow - x.Published) <= TimeSpan.Parse(((int)_timeSpan).ToString())).ToList();
        }
    }
}
