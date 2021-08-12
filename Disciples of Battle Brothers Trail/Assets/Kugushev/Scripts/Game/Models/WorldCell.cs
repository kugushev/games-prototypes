using Kugushev.Scripts.Game.Models.CityInfo;

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
        public CityWorldCell(CityWorldItem cityWorldItem) => CityWorldItem = cityWorldItem;

        public CityWorldItem CityWorldItem { get; }
    }
}