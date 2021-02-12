using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Tao.Project.Console
{
    /// <summary>
    /// 性能指标收集器
    /// </summary>
    public sealed class PerformanceMetricsCollector : IHostedService
    {
        private readonly IPerformanceMetricsCollector _performanceMetricsCollector;
        private readonly IMemoryMetricsCollector _memoryMetricsCollector;
        private readonly INetWorkMetricsCollector _netWorkMetricsCollector;
        private readonly IMetricsDeliverer _metricsDeliverer;
        private readonly TimeSpan _captureInterval;
        private IDisposable _scheduler;

        public PerformanceMetricsCollector(
            IPerformanceMetricsCollector performanceMetricsCollector,
            IMemoryMetricsCollector memoryMetricsCollector,
            INetWorkMetricsCollector netWorkMetricsCollector,
            IMetricsDeliverer metricsDeliverer,
            IOptions<MetricsCollectionOptions> optionsAccessor)
        {
            _performanceMetricsCollector = performanceMetricsCollector;
            _memoryMetricsCollector = memoryMetricsCollector;
            _netWorkMetricsCollector = netWorkMetricsCollector;
            _metricsDeliverer = metricsDeliverer;
            var options = optionsAccessor.Value;
            _captureInterval = options.CaptureInterval;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = new Timer(Callback, null, TimeSpan.FromSeconds(5), _captureInterval);
            return Task.CompletedTask;

            async void Callback(object state)
            {
                var counter = new PerformanceMetrics()
                {
                    Processor = _performanceMetricsCollector.GetUsage(),
                    Memory = _memoryMetricsCollector.GetUsage(),
                    Network = _netWorkMetricsCollector.GetThroughput()
                };
                await _metricsDeliverer.DeliverAsync(counter);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _scheduler?.Dispose();
            return Task.CompletedTask;
        }
    }
}