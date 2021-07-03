using System.Collections.Generic;
using Kugushev.Scripts.Game.Core.Models.WorldUnits;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class BattleDefinition
    {
        public BattleDefinition(PlayerWorldUnit player, WorldUnit enemy)
        {
            Player = player;
            Enemy = enemy;
        }

        public PlayerWorldUnit Player { get; }
        public WorldUnit Enemy { get; }
    }
}