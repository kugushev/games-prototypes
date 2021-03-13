using Kugushev.Scripts.Common.ValueObjects;

namespace Kugushev.Scripts.Mission.ValueObjects.PlayerProperties
{
    public readonly struct GradatedEffects
    {
        public GradatedEffects(GradatedEffectsBuilder gradatedEffectsBuilder)
        {
            UnderCapEffect = gradatedEffectsBuilder.UnderCapEffect;
            Cap = gradatedEffectsBuilder.Cap;
            OverCapEffect = gradatedEffectsBuilder.OverCapEffect;
        }

        public readonly Percentage? UnderCapEffect;
        public readonly float? Cap;
        public readonly Percentage? OverCapEffect;

        public Percentage? GetEffect(float value)
        {
            if (Cap != null)
            {
                if (UnderCapEffect != null && value <= Cap)
                    return UnderCapEffect.Value;

                if (OverCapEffect != null && value > Cap)
                    return OverCapEffect.Value;
            }

            return null;
        }
    }
}