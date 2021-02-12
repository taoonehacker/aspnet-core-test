using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Tao.Project.Console
{
    /// <summary>
    /// 性能指标收集器
    /// </summary>
    public class PerformanceMetricsCollector : IHostedService
    {
        private readonly IPerformanceMetricsCollector _performanceMetricsCollector;
        private readonly IMemoryMetricsCollector _memoryMetricsCollector;
        private readonly INetWorkMetricsCollector _netWorkMetricsCollector;
        private readonly IMetricsDeliverer _metricsDeliverer;
        private IDisposable _scheduler;

        public PerformanceMetricsCollector(
            IPerformanceMetricsCollector performanceMetricsCollector,
            IMemoryMetricsCollector memoryMetricsCollector,
            INetWorkMetricsCollector netWorkMetricsCollector,
            IMetricsDeliverer metricsDeliverer)
        {
            _performanceMetricsCollector = performanceMetricsCollector;
            _memoryMetricsCollector = memoryMetricsCollector;
            _netWorkMetricsCollector = netWorkMetricsCollector;
            _metricsDeliverer = metricsDeliverer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = new Timer(Callback, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
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