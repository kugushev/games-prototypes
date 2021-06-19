using System.Collections.Generic;
using Kugushev.Scripts.Game.Core.Models;

namespace Kugushev.Scripts.Game.Core.Parameters
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