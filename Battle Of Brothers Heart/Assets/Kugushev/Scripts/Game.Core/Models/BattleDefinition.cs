using System.Collections.Generic;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class BattleDefinition
    {
        public BattleDefinition(Party playerParty, Party enemyParty)
        {
            PlayerParty = playerParty;
            EnemyParty = enemyParty;
        }

        public Party PlayerParty { get; }
        public Party EnemyParty { get; }
    }
}