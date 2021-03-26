using System;
using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.ValueObjects.MissionEvents
{
    public readonly struct PlanetCaptured
    {
        public PlanetCaptured(TimeSpan time, Faction newOwner, Faction previousOwner, float overpower)
        {
            Time = time;
            NewOwner = newOwner;
            PreviousOwner = previousOwner;
            Overpower = overpower;
        }

        public readonly TimeSpan Time;
        public readonly Faction NewOwner;
        public readonly Faction PreviousOwner;
        public readonly float Overpower;
    }
}