using System;
using System.Threading;
using System.Threading.Tasks;

using Inverter.Interfaces;

namespace Inverter.Host
{
    public class ConsolePublisher : IPublisher
    {
        public Task Publish(Inverter inverter, Measurement measurement, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[{measurement.CreatedAt.ToLocalTime():yyyy-MM-dd HH:mm}] {measurement.Power}w | {measurement.Temperature}°C");
            return Task.CompletedTask;
        }
    }
}