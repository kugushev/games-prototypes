namespace Kugushev.Scripts.Common.ValueObjects
{
    public readonly struct Degree
    {
        public Degree(float value)
        {
            Value = value;
        }

        public float Value { get; }
    }
}