using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Pipelines.PipelineSteps
{
    public class GamePlatformsPipeline : IPipelineStep<List<Game>, List<Game>>
    {
        private readonly IEnumerable<PlatformType> _platforms;
        public GamePlatformsPipeline(IEnumerable<PlatformType> platforms)
        {
            _platforms = platforms;
        }
        public List<Game> Process(List<Game> input)
        {
            if (_platforms != null && _platforms.Any())
            {
                return input.Where(x => x.PlatformTypes != null).Where(x => x.PlatformTypes.Select(x => x.PlatformTypeId).Intersect(_platforms.Select(x => x.Id)).Any()).ToList();
            }
            return input;
        }
    }
}
