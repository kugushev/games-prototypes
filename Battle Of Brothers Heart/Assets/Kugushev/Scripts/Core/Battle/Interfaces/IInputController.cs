using System;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects;

namespace Kugushev.Scripts.Core.Battle.Interfaces
{
    public interface IInputController
    {
        // Selection - mouse left click
        event Action<PlayerUnit> PlayerUnitSelected;

        // Command - mouse right click
        event Action<Position> GroundCommand;
        event Action<EnemyUnit> EnemyUnitCommand;
    }
}