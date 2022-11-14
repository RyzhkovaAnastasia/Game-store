using System;
namespace OnlineGameStore.DAL.Pipelines.Pipelines
{
    public abstract class GamePipeline<InputType, OutputType> : IPipelineStep<InputType, OutputType>
    {
        public Func<InputType, OutputType> PipelineSteps { get; protected set; }

        public OutputType Process(InputType input)
        {
            return PipelineSteps(input);
        }
    }
}
