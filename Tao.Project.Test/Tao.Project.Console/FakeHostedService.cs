using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Tao.Project.Console
{
    public class FakeHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private IDisposable _tokenSource;

        public FakeHostedService(IHostApplicationLifetime hostApplicationLifetime)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _hostApplicationLifetime.ApplicationStarted.Register(() => System.Console.WriteLine("[{0}]Application started", DateTimeOffset.Now));
            _hostApplicationLifetime.ApplicationStopping.Register(() => System.Console.WriteLine("[{0}]Application is stopping", DateTimeOffset.Now));
            _hostApplicationLifetime.ApplicationStopped.Register(() => System.Console.WriteLine("[{0}]Application stopped.", DateTimeOffset.Now));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5))
                .Token.Register(_hostApplicationLifetime.StopApplication);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource?.Dispose();
            return Task.CompletedTask;
        }
    }
}