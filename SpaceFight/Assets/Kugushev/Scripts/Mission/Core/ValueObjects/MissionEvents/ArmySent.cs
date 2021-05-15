using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.ValueObjects.MissionEvents
{
    public readonly struct ArmySent
    {
        public ArmySent(Faction owner, float power, float remainingPower)
        {
            Owner = owner;
            Power = power;
            RemainingPower = remainingPower;
        }

        public readonly Faction Owner;
        public readonly float Power;
        public readonly float RemainingPower;
    }
}