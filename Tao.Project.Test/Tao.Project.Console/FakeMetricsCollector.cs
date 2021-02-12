using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

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
        private readonly TransportType _transport;
        private readonly Endpoint _deliverTo;

        public FakeMetricsDelivever(IOptions<MetricsCollectionOptions> optionsAccessor)
        {
            var options = optionsAccessor.Value;
            _transport = options.Transport;
            _deliverTo = options.DeliverTo;
        }

        public Task DeliverAsync(PerformanceMetrics counter)
        {
            System.Console.WriteLine($"[{DateTimeOffset.Now}] Deliver performance counter {counter} to {_deliverTo} via {_transport}");
            return Task.CompletedTask;
        }
    }
}