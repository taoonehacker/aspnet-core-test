using System;
using System.Threading.Tasks;

namespace Tao.Project.Console
{
    public class FakeMetricsCollector :
        IPerformanceMetricsCollector,
        IMemoryMetricsCollector,
        INetWorkMetricsCollector
    {
        public int GetUsage()
        {
            return PerformanceMetrics.Create().Processor;
        }

        long IMemoryMetricsCollector.GetUsage()
        {
            return PerformanceMetrics.Create().Memory;
        }

        public long GetThroughput()
        {
            return PerformanceMetrics.Create().Network;
        }
    }

    public class FakeMetricsDelivever : IMetricsDeliverer
    {
        public Task DeliverAsync(PerformanceMetrics counter)
        {
            System.Console.WriteLine($"[{DateTimeOffset.Now}]{counter}");
            return Task.CompletedTask;
        }
    }
}