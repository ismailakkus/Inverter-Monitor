using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Inverter.Interfaces;

using Microsoft.Extensions.Hosting;

namespace Inverter.Host
{
    internal class Service : IHostedService, IDisposable
    {
        private readonly Inverters _inverters;
        private readonly IEnumerable<IPublisher> _publishers;
        private readonly ServiceSettings _settings;
        private Timer _timer;

        public Service(Inverters inverters,
                      IEnumerable<IPublisher> publishers,
                      ServiceSettings settings)
        {
            _inverters = inverters;
            _publishers = publishers;
            _settings = settings;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var inverters = await _inverters.All().ConfigureAwait(false);
            _timer = new Timer(_ => Execute(inverters), null, TimeSpan.Zero, _settings.Interval);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void Execute(IEnumerable<Inverter> inverters)
        {
            foreach(var inverter in inverters)
            {
                var measurement = _inverters.LatestMeasurement(inverter.Id).GetAwaiter().GetResult();
                Task.WhenAll(_publishers.AsParallel().Select(publisher => publisher.Publish(inverter, measurement, CancellationToken.None))).GetAwaiter().GetResult();
            }
        }
    }
}