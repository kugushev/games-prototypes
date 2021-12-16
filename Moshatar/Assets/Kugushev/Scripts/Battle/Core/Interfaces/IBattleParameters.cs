using System.Collections.Generic;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Interfaces
{
    public interface IBattleParameters
    {
        int HeroDamage { get; }
        int HeroDamageSuper { get; }
        int FireBreathDamage { get; }
        int HeroMaxHp { get; }
        int HeroRegeneration { get; }
        int HeroLifestealAmount { get; }
        double HeroInvulnerableSeconds { get; }

        int PlayerSquadSize { get; }
        int TeammateDefaultDamage { get; }
        int TeammateDefaultMaxHp { get; }
        int DefendingPointHealth { get; }
        IReadOnlyList<Vector2> DefendingPoints { get; }

        int EnemyDefaultDamage { get; }
        int EnemyDefaultMaxHp { get; }
        int EnemyBigDamage { get; }
        int EnemyBigMaxHp { get; }
        float EnemyBigAttackRange { get; }
        float EnemySpawnSize { get; }
        int EnemyMaxSquadSize { get; }
        int EnemyMinSquadSize { get; }
        int EnemyMaxBigUnits { get; }
        int EnemyMinBigUnits { get; }
        float EnemyHeroHunterSpawnProbability { get; }
        float EnemyConquerorSpawnProbability { get; }
    }
}