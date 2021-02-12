using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        private readonly Action<ILogger, DateTimeOffset, PerformanceMetrics, Endpoint, TransportType, Exception> _logForDelivery;

        public FakeMetricsDelivever(IOptions<MetricsCollectionOptions> optionsAccessor, ILogger<FakeMetricsDelivever> logger)
        {
            var options = optionsAccessor.Value;
            _transport = options.Transport;
            _deliverTo = options.DeliverTo;
            _logger = logger;
            _logForDelivery = LoggerMessage.Define<DateTimeOffset, PerformanceMetrics, Endpoint, TransportType>(LogLevel.Information, 0, "[{0}]Deliver performance counter {1} to {2} via {3}");
        }

        public Task DeliverAsync(PerformanceMetrics counter)
        {
            _logForDelivery(_logger, DateTimeOffset.Now, counter, _deliverTo, _transport, null);
            System.Console.WriteLine($"[{DateTimeOffset.Now}] Deliver performance counter {counter} to {_deliverTo} via {_transport}");
            return Task.CompletedTask;
        }
    }
}