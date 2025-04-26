using System.Threading.Tasks;

namespace MonadsExtensions.Pipelines
{
    public interface IAsyncPipeline<in TInput, TOutput>
    {
        Task<TOutput> ExecuteAsync(TInput input);
    }
}
