using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Pipelines.SortPipelineSteps
{
    public class GameSortSkipPipeline : IPipelineStep<List<Game>, List<Game>>
    {
        public List<Game> Process(List<Game> input)
        {
            return input;
        }
    }
}
