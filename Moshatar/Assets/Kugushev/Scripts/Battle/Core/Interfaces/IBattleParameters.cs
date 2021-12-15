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
        int PlayerDefaultDamage { get; }
        int PlayerDefaultMaxHp { get; }
        int PlayerMaxSquadSize { get; }

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
    }
}