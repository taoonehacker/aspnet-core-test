using System.Threading.Tasks;

namespace Tao.Project.Console
{
    public interface IPerformanceMetricsCollector
    {
        /// <summary>
        /// CPU使用情况
        /// </summary>
        /// <returns></returns>
        int GetUsage();
    }

    public interface IMemoryMetricsCollector
    {
        /// <summary>
        /// 内存使用情况
        /// </summary>
        /// <returns></returns>
        long GetUsage();
    }

    public interface INetWorkMetricsCollector
    {
        /// <summary>
        /// 网络吞吐量
        /// </summary>
        /// <returns></returns>
        long GetThroughput();
    }

    public interface IMetricsDeliverer
    {
        Task DeliverAsync(PerformanceMetrics counter);
    }
}