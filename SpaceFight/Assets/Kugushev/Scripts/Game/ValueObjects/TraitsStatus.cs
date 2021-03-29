using System;

namespace Kugushev.Scripts.Game.ValueObjects
{
    public readonly struct TraitsStatus
    {
        public TraitsStatus(bool business, bool greed, bool lust, bool brute, bool vanity)
        {
            Business = business;
            Greed = greed;
            Lust = lust;
            Brute = brute;
            Vanity = vanity;
        }

        public bool Business { get; }
        public bool Greed { get; }
        public bool Lust { get; }
        public bool Brute { get; }
        public bool Vanity { get; }
        public bool IsAllRevealed => Business && Greed && Lust && Brute && Vanity;

        public TraitsStatus RevealOne(Traits politicianTraits, int intel)
        {
            if (IsAllRevealed)
                return this;

            var business = Business;
            var greed = Greed;
            var lust = Lust;
            var brute = Brute;
            var vanity = Vanity;

            var revealed =
                RevealTrait(ref business, intel, politicianTraits.Business) ||
                RevealTrait(ref greed, intel, politicianTraits.Greed) ||
                RevealTrait(ref lust, intel, politicianTraits.Lust) ||
                RevealTrait(ref brute, intel, politicianTraits.Brute) ||
                RevealTrait(ref vanity, intel, politicianTraits.Vanity);

            if (revealed)
                return new TraitsStatus(business, greed, lust, brute, vanity);

            return this;
        }

        private bool RevealTrait(ref bool isTraitRevealed, int intel, int politicianTrait)
        {
            if (!isTraitRevealed && intel >= Math.Abs(politicianTrait))
            {
                isTraitRevealed = true;
                return true;
            }

            return false;
        }
    }
}