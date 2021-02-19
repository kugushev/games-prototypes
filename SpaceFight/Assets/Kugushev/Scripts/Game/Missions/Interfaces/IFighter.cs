using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Missions.Enums;

namespace Kugushev.Scripts.Game.Missions.Interfaces
{
    public interface IFighter
    {
        Position Position { get; }
        Faction Faction { get; }
        bool CanBeAttacked { get; }
        FightRoundResult SufferFightRound(Faction enemyFaction, int damage = GameConstants.UnifiedDamage);
    }
}