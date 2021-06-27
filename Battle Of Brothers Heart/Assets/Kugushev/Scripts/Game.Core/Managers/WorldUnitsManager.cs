using System.Collections.Generic;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.Core.Managers
{
    public class WorldUnitsManager
    {
        public WorldUnit Player { get; } = new WorldUnit(
            GameConstants.Units.PlayerUnitStartPosition,
            GameConstants.Units.PlayerUnitStartDirection);

        public IReadOnlyList<WorldUnit> Bandits { get; } = new[]
        {
            new WorldUnit(new Position(new Vector2(5, 5)), Direction2d.Down)
        };
    }
}