using System;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Models.Fighters
{
    public class HeroFighter : BaseFighter
    {
        private const int MaxHp = 12;
        private const int Regeneration = 2;
        private const int LifestealAmount = 1;
        private const double InvulnerableSeconds = 1;

        private DateTime _lastHitTime = DateTime.MinValue;
        
        public Vector3 HeadPosition { get; set; } // todo: remove this hack

        protected override bool SimplifiedSuffering => false;

        public HeroFighter(Position battlefieldPosition, Battlefield battlefield)
            : base(battlefieldPosition, new Character(MaxHp, 0), battlefield)
        {
        }

        public override void Suffer(int damage)
        {
            var now = DateTime.Now;
        
            if ((now - _lastHitTime).TotalSeconds > InvulnerableSeconds)
            {
                _lastHitTime = now;
                base.Suffer(damage);
            }
        }

        public void UpdatePosition(Position position)
        {
            PositionImpl.Value = position;
        }

        public void Regenerate()
        {
            if (Character.HP.Value < Character.MaxHP)
                Character.Regenerate(Regeneration);
        }

        public void Lifesteal()
        {
            if (Character.HP.Value < Character.MaxHP)
                Character.Regenerate(LifestealAmount);
        }
        
    }
}