using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.ValueObjects.MissionEvents
{
    public readonly struct ArmyDestroyedOnSiege
    {
        public ArmyDestroyedOnSiege(Faction destroyer, Faction victim, float armyStartPower, float planetPowerLeft)
        {
            PlanetPowerLeft = planetPowerLeft;
            Destroyer = destroyer;
            Victim = victim;
            ArmyStartPower = armyStartPower;
        }


        public readonly Faction Destroyer;
        public readonly Faction Victim;
        public readonly float ArmyStartPower;
        public readonly float PlanetPowerLeft;
    }
}