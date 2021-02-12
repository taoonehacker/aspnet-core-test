using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tao.Project.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // new HostBuilder().ConfigureServices(svcs => svcs.AddSingleton<IHostedService, PerformanceMetricsCollector>())
            //     .Build()
            //     .Run();

            var collector = new FakeMetricsCollector();
            new HostBuilder()
                .ConfigureServices(svcs =>
                    svcs.AddSingleton<IPerformanceMetricsCollector>(collector)
                        .AddSingleton<IMemoryMetricsCollector>(collector)
                        .AddSingleton<INetWorkMetricsCollector>(collector)
                        .AddSingleton<IMetricsDeliverer, FakeMetricsDelivever>()
                        .AddHostedService<PerformanceMetricsCollector>())
                .Build()
                .Run();
        }
    }
}