using System;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Common.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.Interfaces
{
    public interface IInputController
    {
        // Selection - mouse left click
        event Action<PlayerFighter> PlayerUnitSelected;

        // Command - mouse right click
        event Action<Position> GroundCommand;
        event Action<EnemyFighter> EnemyUnitCommand;
    }
}