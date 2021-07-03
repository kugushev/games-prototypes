using System.Collections.Generic;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Models;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Game.Core.Managers
{
    public class WorldUnitsManager
    {
        private readonly ReactiveCollection<WorldUnit> _units;

        public WorldUnitsManager()
        {
            Player = new WorldUnit(
                GameConstants.Units.PlayerUnitStartPosition,
                GameConstants.Units.PlayerUnitStartDirection,
                new Party(new[]
                {
                    new Character(), new Character(), new Character()
                }));

            _units = new ReactiveCollection<WorldUnit>
            {
                new WorldUnit(new Position(new Vector2(5, 5)), Direction2d.Down, new Party(new[]
                    {
                        new Character(), new Character(), new Character(), new Character()
                    }
                ))
            };
        }

        public WorldUnit Player { get; }

        public IReadOnlyReactiveCollection<WorldUnit> Units => _units;

        public void RemoveUnit(WorldUnit unit)
        {
            _units.Remove(unit);
        }
    }
}