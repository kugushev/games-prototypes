using System;

namespace Kugushev.Scripts.Game.ValueObjects
{
    // todo: make it poolable class
    public class IntrigueRecord
    {
        public Intrigue Intrigue { get; }

        public IntrigueRecord(Intrigue intrigue)
        {
            Intrigue = intrigue;
        }
    }
}