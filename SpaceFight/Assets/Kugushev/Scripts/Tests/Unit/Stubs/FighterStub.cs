using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;

namespace Kugushev.Scripts.Tests.Unit.Stubs
{
    // todo: add NSubstitude
    public class FighterStub : IFighter
    {
        public const float StartPower = 50f;

        public float Power { get; private set; } = StartPower;

        public bool Active => true;
        public Position Position { get; set; }
        public Faction Faction => Faction.Red;
        public bool CanBeAttacked => true;

        public FightRoundResult SufferFightRound(Faction enemyFaction, float damage = GameplayConstants.UnifiedDamage)
        {
            Power += damage;
            return FightRoundResult.StillAlive;
        }
    }
}