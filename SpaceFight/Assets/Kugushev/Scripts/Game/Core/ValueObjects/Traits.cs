namespace Kugushev.Scripts.Game.ValueObjects
{
    public readonly struct Traits
    {
        public Traits(int business, int greed, int lust, int brute, int vanity)
        {
            Business = business;
            Greed = greed;
            Lust = lust;
            Brute = brute;
            Vanity = vanity;
        }

        public int Business { get; }
        public int Greed { get; }
        public int Lust { get; }
        public int Brute { get; }
        public int Vanity { get; }
    }
}