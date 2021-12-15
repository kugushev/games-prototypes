using Kugushev.Scripts.Battle.Core.Interfaces;

namespace Kugushev.Scripts.Battle.Core.ValueObjects.Parameters
{
    public class MusouBattleParameters : IBattleParameters
    {
        public int HeroDamage => 4;
        public int HeroDamageSuper => 20;
        public int FireBreathDamage => 10;
        public int HeroMaxHp => 12;
        public int HeroRegeneration => 2;
        public int HeroLifestealAmount => 1;
        public double HeroInvulnerableSeconds => 1;

        public int PlayerSquadSize => 4;
        public int TeammateDefaultDamage => 3;
        public int TeammateDefaultMaxHp => 350;

        public int EnemyMaxSquadSize => 22;
        public int EnemyMinSquadSize => 12;
        public int EnemyMaxBigUnits => 5;
        public int EnemyMinBigUnits => 3;
        public int EnemyDefaultDamage => 1;
        public int EnemyDefaultMaxHp => 4;
        public int EnemyBigDamage => 4;
        public int EnemyBigMaxHp => 80;
        public float EnemyBigAttackRange => 3f;
        public float EnemySpawnSize => 8f;
    }
}