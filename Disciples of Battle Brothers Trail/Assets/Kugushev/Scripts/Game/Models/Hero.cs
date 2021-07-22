namespace Kugushev.Scripts.Game.Models
{
    public class Hero
    {
        public static Hero Instance { get; } = new Hero();

        private Hero()
        {
        }
        // todo: shared state, gold, team, etc.
    }
}