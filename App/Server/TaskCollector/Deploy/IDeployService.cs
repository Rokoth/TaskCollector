using System.Threading.Tasks;

namespace TaskCollector.Deploy
{
    public interface IDeployService
    {
        Task Deploy(int? num = null);
    }
}