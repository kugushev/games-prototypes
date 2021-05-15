namespace Kugushev.Scripts.Mission.Core.ValueObjects
{
    public readonly struct Production
    {
        public Production(float amount)
        {
            Amount = amount;
        }

        public float Amount { get; }
    }
}