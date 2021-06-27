using Kugushev.Scripts.Game.Core.Models;

namespace Kugushev.Scripts.Game.Core.Managers
{
    public class WorldUnitsManager
    {
        public WorldUnit Player { get; } = new WorldUnit(
            GameConstants.Units.PlayerUnitStartPosition,
            GameConstants.Units.PlayerUnitStartDirection);
    }
}