using System;
using Kugushev.Scripts.Core.Battle.ValueObjects;

namespace Kugushev.Scripts.Core.Battle
{
    public static class BattleConstants
    {
        public const float UnitSpeed = 5f;
        public const float UnitToTargetEpsilon = 0.1f;
        public const float SwordAttackRange = 2f;
        public static readonly TimeSpan SwordAttackCooldown = TimeSpan.FromSeconds(1);
    }
}