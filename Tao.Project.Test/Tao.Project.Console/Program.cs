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

            new HostBuilder()
                .ConfigureServices(svcs => svcs.AddHostedService<PerformanceMetricsCollector>())
                .Build()
                .Run();
        }
    }
}