using System;
using Kugushev.Scripts.Game.Enums;

namespace Kugushev.Scripts.Game.Models.HeroInfo
{
    public class Teammate
    {
        public Teammate(BattleUnit battleUnit)
        {
            BattleUnit = battleUnit;
        }

        public BattleUnit BattleUnit { get; }

        public int Kills { get; set; }
        public int Relation { get; set; }
    }
}