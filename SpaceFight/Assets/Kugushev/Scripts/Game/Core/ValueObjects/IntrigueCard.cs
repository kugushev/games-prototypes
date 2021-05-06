namespace Kugushev.Scripts.Game.Core.ValueObjects
{
    // todo: make it poolable class
    public class IntrigueCard
    {
        public Intrigue Intrigue { get; }

        public IntrigueCard(Intrigue intrigue)
        {
            Intrigue = intrigue;
        }
    }
}