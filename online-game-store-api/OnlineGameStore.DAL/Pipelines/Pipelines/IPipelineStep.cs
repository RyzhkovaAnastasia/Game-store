namespace OnlineGameStore.DAL.Pipelines.Pipelines
{
    public interface IPipelineStep<in InputType, out OutputType>
    {
        OutputType Process(InputType input);
    }
}
