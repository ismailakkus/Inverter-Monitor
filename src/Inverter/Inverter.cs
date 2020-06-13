namespace Inverter
{
    public class Inverter
    {
        public Inverter(string id)
        {
            Id = InverterId.Create(id);
        }

        public InverterId Id { get; }
    }
}