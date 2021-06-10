using System;
using Kugushev.Scripts.Core.Battle.ValueObjects;

namespace Kugushev.Scripts.Core.Battle
{
    public static class BattleConstants
    {
        public const float UnitSpeed = 5f;
        public const float UnitToTargetEpsilon = 0.1f;

        public static readonly TimeSpan HurtInterruptionTime = TimeSpan.FromMilliseconds(400);

        public const float SwordAttackRange = 3f;
        public static readonly TimeSpan SwordAttackCooldown = TimeSpan.FromSeconds(1);
        public static readonly TimeSpan SwordAttackBeforeHurtTime = TimeSpan.FromMilliseconds(300);
        public static readonly TimeSpan SwordAttackAfterHurtTime = TimeSpan.FromMilliseconds(300);
    }
}