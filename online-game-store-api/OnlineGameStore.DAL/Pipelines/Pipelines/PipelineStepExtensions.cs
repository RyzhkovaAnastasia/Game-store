using System;

namespace OnlineGameStore.DAL.Pipelines.Pipelines
{
    public static class PipelineStepExtensions
    {
        public static OutputType Step<InputType, OutputType>(this InputType input, IPipelineStep<InputType, OutputType> step)
        {
            if (step != null)
            {
                return step.Process(input);
            }
            throw new ArgumentException("Step was null");
        }
    }
}
