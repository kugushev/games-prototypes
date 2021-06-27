using Kugushev.Scripts.Game.Core.Models;

namespace Kugushev.Scripts.Game.Core.Managers
{
    public class UnitsManager
    {
        public WorldUnit Player { get; } = new WorldUnit(
            GameConstants.Units.PlayerUnitStartPosition,
            GameConstants.Units.PlayerUnitStartDirection);
    }
}