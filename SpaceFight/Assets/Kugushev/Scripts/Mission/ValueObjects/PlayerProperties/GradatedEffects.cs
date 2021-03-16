using Kugushev.Scripts.Common.ValueObjects;

namespace Kugushev.Scripts.Mission.ValueObjects.PlayerProperties
{
    public readonly struct GradatedEffects
    {
        private readonly Percentage? _underCapEffect;
        private readonly float? _lowCap;
        private readonly float? _highCap;
        private readonly Percentage? _overCapEffect;

        public GradatedEffects(GradatedEffectsBuilder gradatedEffectsBuilder)
        {
            _underCapEffect = gradatedEffectsBuilder.UnderCapEffect;
            _lowCap = gradatedEffectsBuilder.LowCap;
            _highCap = gradatedEffectsBuilder.HighCap;
            _overCapEffect = gradatedEffectsBuilder.OverCapEffect;
        }

        public Percentage? GetEffect(float value)
        {
            if (_lowCap != null && _underCapEffect != null && value <= _lowCap)
                return _underCapEffect.Value;

            if (_highCap != null && _overCapEffect != null && value > _lowCap)
                return _overCapEffect.Value;

            return null;
        }
    }
}