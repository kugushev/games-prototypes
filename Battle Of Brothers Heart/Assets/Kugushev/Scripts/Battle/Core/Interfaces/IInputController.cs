﻿using System;
using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Common.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.Interfaces
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