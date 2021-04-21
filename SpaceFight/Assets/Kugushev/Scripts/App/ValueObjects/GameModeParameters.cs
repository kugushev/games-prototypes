namespace Kugushev.Scripts.App.ValueObjects
{
    public readonly struct GameModeParameters
    {
        public readonly int Seed;

        public GameModeParameters(int seed)
        {
            Seed = seed;
        }
    }
}