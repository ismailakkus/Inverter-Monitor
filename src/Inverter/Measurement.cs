using System;

namespace Inverter
{
    public class Measurement
    {
        public DateTimeOffset CreatedAt { get; }
        public float Power { get; }
        public float Temperature { get; }

        public Measurement(DateTimeOffset createdAt, 
                           float power, 
                           float temperature)
        {
            CreatedAt = createdAt;
            Power = power;
            Temperature = temperature;
        }
    }
}