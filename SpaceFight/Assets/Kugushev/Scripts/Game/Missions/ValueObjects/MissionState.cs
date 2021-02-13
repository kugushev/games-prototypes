using System.Collections.Generic;
using Kugushev.Scripts.Game.Common.Interfaces;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;

namespace Kugushev.Scripts.Game.Missions.ValueObjects
{
    public readonly struct MissionState
    {
        public MissionState(PlanetarySystem currentPlanetarySystem, ConflictParty red, ConflictParty green)
        {
            CurrentPlanetarySystem = currentPlanetarySystem;
            Red = red;
            Green = green;
        }

        public PlanetarySystem CurrentPlanetarySystem { get; }
        public ConflictParty Red { get; }
        public ConflictParty Green { get; }

        public void Setup()
        {
            Green.Commander.AssignFleet(Green.Fleet, Green.Faction);
            Red.Commander.AssignFleet(Red.Fleet, Red.Faction);
        }

        public void Dispose()
        {
            CurrentPlanetarySystem.Dispose();
            Red.Dispose();
            Green.Dispose();
        }
    }
}