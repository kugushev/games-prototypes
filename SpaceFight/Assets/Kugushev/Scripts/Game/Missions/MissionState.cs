using System.Collections.Generic;
using Kugushev.Scripts.Game.Common.Interfaces;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;

namespace Kugushev.Scripts.Game.Missions
{
    public readonly struct MissionState
    {
        public PlanetarySystem CurrentPlanetarySystem { get; }
        public IReadOnlyList<IAIAgent> AIAgents { get; }
    }

    public readonly struct ConflictParty
    {
        public Faction Faction { get; }
    }
}