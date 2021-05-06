namespace Kugushev.Scripts.Game.Core.ValueObjects
{
    public readonly struct PoliticsParameters
    {
        public PoliticsParameters(IntriguesSet intriguesSet)
        {
            IntriguesSet = intriguesSet;
        }

        public IntriguesSet IntriguesSet { get; }
    }
}