using System;

namespace Inverter
{
    public class InverterId
    {
        private string Value { get; }

        private InverterId(string value)
        {
            Value = value ?? string.Empty;
        }

        public static implicit operator string(InverterId c) => c?.Value ?? string.Empty;
        public static implicit operator InverterId(string s) => new InverterId(s);
        public static InverterId Create(string value) => new InverterId(value);

        public override string ToString() => Value;
        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj)
        {
            if(Value == null || obj == null)
                return false;

            if(obj.GetType() != GetType())
                return false;

            if(obj is string s)
            {
                return string.Equals(Value, s, StringComparison.Ordinal);
            }

            var otherString = $"{obj}";
            return string.Equals(Value, otherString, StringComparison.Ordinal);
        }
    }
}