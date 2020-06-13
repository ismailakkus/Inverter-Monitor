using System.Threading;
using System.Threading.Tasks;

namespace Inverter.Interfaces
{
    public interface IPublisher
    {
        Task Publish(Inverter inverter, Measurement measurement, CancellationToken cancellationToken);
    }
}