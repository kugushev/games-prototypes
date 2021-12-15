namespace Kugushev.Scripts.Battle.Core.Interfaces
{
    public interface IBattleParameters
    {
        int HeroDamage { get; }
        int HeroDamageSuper { get; }
        int FireBreathDamage { get; }

        int PlayerSquadSize { get; }
        int PlayerDefaultDamage { get; }
        int PlayerDefaultMaxHp { get; }
        int PlayerMaxSquadSize { get; }

        int EnemyMaxSquadSize { get; }
        int EnemyDefaultDamage { get; }
        int EnemyDefaultMaxHp { get; }
        int EnemyBigDamage { get; }
        int EnemyBigMaxHp { get; }
        float EnemyBigAttackRange { get; }
        float EnemySpawnSize { get; }
    }
}