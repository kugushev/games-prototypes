namespace Kugushev.Scripts.Mission.ValueObjects.PlayerProperties
{
    public readonly struct FleetProperties
    {
        public FleetProperties(FleetPropertiesBuilder builder)
        {
            SiegeMultiplier = 1 + builder.SiegeDamageMultiplier;
            FightMultiplier = 1 + builder.FightDamageMultiplier;
            FightDamageMultiplication = new GradatedEffects(builder.FightDamageMultiplication);
            FightProtectionMultiplication = new GradatedEffects(builder.FightProtectionMultiplication);
            SiegeDamageMultiplication = new GradatedEffects(builder.SiegeDamageMultiplication);
            DeathStrike = builder.DeathStrike;
        }

        public readonly float? SiegeMultiplier;
        public readonly float? FightMultiplier;
        public readonly GradatedEffects FightDamageMultiplication;
        public readonly GradatedEffects FightProtectionMultiplication;
        public readonly GradatedEffects SiegeDamageMultiplication;
        public readonly float? DeathStrike;
    }
}