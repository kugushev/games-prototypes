namespace Kugushev.Scripts.Mission.Core.ValueObjects
{
    public readonly struct Power
    {
        public Power(float amount)
        {
            Amount = amount;
        }

        public float Amount { get; }
    }
}