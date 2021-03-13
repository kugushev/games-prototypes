using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.ValueObjects.MissionEvents
{
    public readonly struct PlanetCaptured
    {
        public PlanetCaptured(Faction newOwner, Faction previousOwner, float overpower)
        {
            NewOwner = newOwner;
            PreviousOwner = previousOwner;
            Overpower = overpower;
        }

        public readonly Faction NewOwner;
        public readonly Faction PreviousOwner;
        public readonly float Overpower;
    }
}