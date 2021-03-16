using System;

namespace Kugushev.Scripts.Mission.ValueObjects.PlayerProperties
{
    public struct FleetPropertiesBuilder
    {
        public float SiegeDamageMultiplier;
        public float FightDamageMultiplier;
        public GradatedEffectsBuilder FightDamageMultiplication;
        public GradatedEffectsBuilder FightProtectionMultiplication;
        public GradatedEffectsBuilder SiegeDamageMultiplication;
        public float? DeathStrike;
    }
}