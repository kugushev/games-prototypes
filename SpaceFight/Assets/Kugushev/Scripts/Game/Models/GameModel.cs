namespace Kugushev.Scripts.Game.Models
{
    public class GameModel
    {
        public GameModel(Parliament parliament)
        {
            Parliament = parliament;
        }

        public Parliament Parliament { get; }
    }
}