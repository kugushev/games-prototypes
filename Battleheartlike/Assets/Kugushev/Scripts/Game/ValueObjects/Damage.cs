namespace Kugushev.Scripts.Game.ValueObjects
{
    public readonly struct Damage
    {
        public int Amount { get; }

        public Damage(int amount)
        {
            Amount = amount;
        }
    }
}