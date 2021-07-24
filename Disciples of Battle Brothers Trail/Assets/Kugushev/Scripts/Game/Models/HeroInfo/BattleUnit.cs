using System;
using Kugushev.Scripts.Game.Enums;

namespace Kugushev.Scripts.Game.Models.HeroInfo
{
    public class BattleUnit
    {
        public BattleUnit(string name, int lvl, AttackType attackType, int damage, int maxHitPoints)
        {
            Name = name;
            Lvl = lvl;
            AttackType = attackType;
            Damage = damage;
            HitPoints = maxHitPoints;
        }

        public string Name { get; }
        public int Lvl { get; }
        public AttackType AttackType { get; }
        public int Damage { get; }
        public int HitPoints { get; set; }
    }
}