using System;

namespace Inverter
{
    public class Measurement
    {
        public DateTimeOffset CreatedAt { get; }
        public float Power { get; }
        public float Temperature { get; }
        public Fault? Fault { get; }

        public Measurement(DateTimeOffset createdAt, 
                           float power, 
                           float temperature,
                           Fault? fault)
        {
            CreatedAt = createdAt;
            Power = power;
            Temperature = temperature;
            Fault = fault;
        }
    }

    public enum Fault
    {
        Unknown = 0,
    }
}