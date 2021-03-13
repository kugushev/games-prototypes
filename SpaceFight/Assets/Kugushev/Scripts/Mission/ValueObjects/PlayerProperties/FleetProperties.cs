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
        }

        public readonly float? SiegeMultiplier;
        public readonly float? FightMultiplier;
        public readonly GradatedEffects FightDamageMultiplication;
        public readonly GradatedEffects FightProtectionMultiplication;
    }
}