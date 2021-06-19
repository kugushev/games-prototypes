using System;
using System.Collections.Generic;

namespace Kugushev.Scripts.Battle.Core
{
    public static class BattleConstants
    {
        public const int UnitMaxHitPoints = 20;

        public const float UnitRadius = 1f;
        public const float UnitSpeed = 2f;
        public const float UnitToTargetEpsilon = 0.1f;

        public static readonly TimeSpan HurtInterruptionTime = TimeSpan.FromMilliseconds(400);

        public const float SwordAttackRange = 1.5f;
        public const int SwordAttackDamage = 1;
        public static readonly TimeSpan SwordAttackCooldown = TimeSpan.FromSeconds(1);
        public static readonly TimeSpan SwordAttackBeforeHurtTime = TimeSpan.FromMilliseconds(300);
        public static readonly TimeSpan SwordAttackAfterHurtTime = TimeSpan.FromMilliseconds(300);

        public const float PlayerSquadLine = 0f;
        public const float EnemySquadLine = 8f;
        public static readonly IReadOnlyList<float> UnitsPositionsInRow = new[] {-3f, -1f, 1f, 3f};

        public const float AIAggroResetMultiplier = 1f;
    }
}