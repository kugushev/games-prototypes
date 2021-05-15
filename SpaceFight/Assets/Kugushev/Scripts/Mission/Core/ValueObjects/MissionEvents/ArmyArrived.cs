using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.ValueObjects.MissionEvents
{
    public readonly struct ArmyArrived
    {
        public ArmyArrived(Faction owner, float power)
        {
            Owner = owner;
            Power = power;
        }

        public readonly Faction Owner;
        public readonly float Power;
    }
}