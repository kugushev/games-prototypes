namespace Kugushev.Scripts.Game.Models
{
    public abstract class WorldCell
    {
    }

    public class GrasslandWorldCell : WorldCell
    {
        public static GrasslandWorldCell Instance { get; } = new GrasslandWorldCell();

        private GrasslandWorldCell()
        {
        }
    }

    public class CityWorldCell : WorldCell
    {
        public CityWorldCell(City city) => City = city;

        public City City { get; }
    }
}