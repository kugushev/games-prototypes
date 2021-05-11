namespace Kugushev.Scripts.App.Core.ContextManagement.Parameters
{
    public readonly struct GameParameters
    {
        public readonly int Seed;

        public GameParameters(int seed)
        {
            Seed = seed;
        }
    }
}