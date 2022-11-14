using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Pipelines.PipelineSteps
{
    public class GameNamePipeline : IPipelineStep<List<Game>, List<Game>>
    {
        private readonly string _name;
        public GameNamePipeline(string name)
        {
            _name = name;
        }

        public List<Game> Process(List<Game> input)
        {
            if (_name != null && _name.Length >= 3)
            {
                return input.Where(x => x.Name.ToLower().Contains(_name.ToLower())).ToList();
            }
            return input;
        }
    }
}
