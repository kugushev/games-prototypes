using System.Collections.Generic;
using Kugushev.Scripts.Core.Game.Models;

namespace Kugushev.Scripts.Core.Game.Parameters
{
    public class BattleParameters
    {
        public BattleParameters(IReadOnlyList<Teammate> team, IReadOnlyList<Enemy> enemies)
        {
            Team = team;
            Enemies = enemies;
        }

        public IReadOnlyList<Teammate> Team { get; private set; }
        public IReadOnlyList<Enemy> Enemies { get; private set; }
        
        // todo: add BattleResult
    }
}