using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Tao.Project.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // new HostBuilder().ConfigureServices(svcs => svcs.AddSingleton<IHostedService, PerformanceMetricsCollector>())
            //     .Build()
            //     .Run();

            // var collector = new FakeMetricsCollector();
            // new HostBuilder()
            //     .ConfigureHostConfiguration(builder => builder.AddCommandLine(args))
            //     .ConfigureAppConfiguration((context, builder) => builder
            //         .AddJsonFile("appSettings.json", optional: false)
            //         .AddJsonFile($"appSettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true))
            //     .ConfigureServices((context, svcs) =>
            //         svcs.AddSingleton<IPerformanceMetricsCollector>(collector)
            //             .AddSingleton<IMemoryMetricsCollector>(collector)
            //             .AddSingleton<INetWorkMetricsCollector>(collector)
            //             .AddSingleton<IMetricsDeliverer, FakeMetricsDelivever>()
            //             .AddHostedService<PerformanceMetricsCollector>()
            //             .AddOptions().Configure<MetricsCollectionOptions>(context.Configuration.GetSection("MetricsCollection"))
            //     )
            //     .ConfigureLogging((context, builder) => builder
            //         .AddConfiguration(context.Configuration.GetSection("Logging")))
            //     .Build()
            //     .Run();

            new HostBuilder()
                .ConfigureServices(svcs => svcs.AddHostedService<FakeHostedService>())
                .Build()
                .Run();
        }
    }
}