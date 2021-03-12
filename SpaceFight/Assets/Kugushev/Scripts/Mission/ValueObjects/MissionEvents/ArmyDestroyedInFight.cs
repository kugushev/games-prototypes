using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.ValueObjects.MissionEvents
{
    public readonly struct ArmyDestroyedInFight
    {
        public ArmyDestroyedInFight(Faction destroyer, Faction victim, float overpower)
        {
            Destroyer = destroyer;
            Victim = victim;
            Overpower = overpower;
        }

        public readonly Faction Destroyer;
        public readonly Faction Victim;
        public readonly float Overpower;
    }
}