namespace Kugushev.Scripts.App.ValueObjects
{
    public readonly struct GameContextParameters
    {
        public readonly int Seed;

        public GameContextParameters(int seed)
        {
            Seed = seed;
        }
    }
}