using System.Collections.Generic;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class BattleDefinition
    {
        public BattleDefinition(WorldUnit player, WorldUnit enemy)
        {
            Player = player;
            Enemy = enemy;
        }

        public WorldUnit Player { get; }
        public WorldUnit Enemy { get; }
    }
}