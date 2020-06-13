using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inverter.Interfaces
{
    public interface Inverters
    {
        Task<IReadOnlyList<Inverter>> All();
        Task<Measurement> LatestMeasurement(InverterId id);
    }
}