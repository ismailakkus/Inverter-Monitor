using System.Threading;
using System.Threading.Tasks;

using Inverter.Interfaces;

using Serilog;

namespace Inverter.Host
{
    public class LoggingPublisher : IPublisher
    {
        private readonly ILogger _logger;

        public LoggingPublisher(ILogger logger)
        {
            _logger = logger;
        }

        public Task Publish(Inverter inverter, Measurement measurement, CancellationToken cancellationToken)
        {
            _logger.Information("at {creationDate} {inverter} at {temperature}°C generated {power}w", measurement.CreatedAt, inverter.Id.ToString(), measurement.Temperature, measurement.Power);
            return Task.CompletedTask;
        }
    }
}