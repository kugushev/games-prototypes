using System.Collections.Generic;
using Kugushev.Scripts.Battle.Core.Interfaces;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.ValueObjects.Parameters
{
    public class TogBattleParameters : IBattleParameters
    {
        public int HeroDamage => 4;
        public int HeroDamageSuper => 12;
        public int FireBreathDamage => 3;
        public int HeroMaxHp => 12;
        public int HeroRegeneration => 2;
        public int HeroLifestealAmount => 1;
        public double HeroInvulnerableSeconds => 1;


        public int PlayerSquadSize => 10;
        public int TeammateDefaultDamage => 3;
        public int TeammateDefaultMaxHp => 40;
        public int DefendingPointHealth => 80;

        public IReadOnlyList<Vector2> DefendingPoints { get; } = new[]
        {
            new Vector2(-9, 10),
            new Vector2(0, 10),
            new Vector2(9, 10),
        };

        public int EnemyMaxSquadSize => 16;
        public int EnemyMinSquadSize => 10;
        public int EnemyMaxBigUnits => 2;
        public int EnemyMinBigUnits => 1;
        public int EnemyDefaultDamage => 4;
        public int EnemyDefaultMaxHp => 12;
        public int EnemyBigDamage => 20;
        public int EnemyBigMaxHp => 60;
        public float EnemyBigAttackRange => 3f;
        public float EnemySpawnSize => 8f;
        public float EnemyHeroHunterSpawnProbability => 0.15f;
        public float EnemyConquerorSpawnProbability => 0.1f;
    }
}