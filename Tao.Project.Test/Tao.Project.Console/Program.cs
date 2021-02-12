﻿using Microsoft.Extensions.Configuration;
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
                .ConfigureAppConfiguration(builder => builder.AddJsonFile("appSettings.json"))
                .ConfigureServices((context, svcs) =>
                    svcs.AddSingleton<IPerformanceMetricsCollector>(collector)
                        .AddSingleton<IMemoryMetricsCollector>(collector)
                        .AddSingleton<INetWorkMetricsCollector>(collector)
                        .AddSingleton<IMetricsDeliverer, FakeMetricsDelivever>()
                        .AddHostedService<PerformanceMetricsCollector>()
                        .AddOptions().Configure<MetricsCollectionOptions>(context.Configuration.GetSection("MetricsCollection"))
                )
                .Build()
                .Run();
        }
    }
}