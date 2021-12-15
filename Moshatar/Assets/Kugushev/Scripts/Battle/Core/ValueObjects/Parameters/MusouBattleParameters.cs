using Kugushev.Scripts.Battle.Core.Interfaces;

namespace Kugushev.Scripts.Battle.Core.ValueObjects.Parameters
{
    public class MusouBattleParameters : IBattleParameters
    {
        public int HeroDamage => 4;
        public int HeroDamageSuper => 20;
        public int FireBreathDamage => 10;

        public int PlayerSquadSize => 4;
        public int PlayerDefaultDamage => 3;
        public int PlayerDefaultMaxHp => 350;
        public int PlayerMaxSquadSize => 4;

        public int EnemyMaxSquadSize => 22;
        public int EnemyDefaultDamage => 1;
        public int EnemyDefaultMaxHp => 4;
        public int EnemyBigDamage => 4;
        public int EnemyBigMaxHp => 80;
        public float EnemyBigAttackRange => 3f;
        public float EnemySpawnSize => 8f;
    }
}