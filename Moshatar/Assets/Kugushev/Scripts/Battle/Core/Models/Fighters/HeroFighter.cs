﻿using System;
using Kugushev.Scripts.Battle.Core.Services;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Models.Fighters
{
    public class HeroFighter : BaseFighter
    {
        private readonly BattleGameplayManager _battleGameplayManager;
        private DateTime _lastHitTime = DateTime.MinValue;

        public Vector3 HeadPosition { get; set; } // todo: remove this hack

        protected override bool SimplifiedSuffering => false;

        public HeroFighter(Position battlefieldPosition, Battlefield battlefield,
            BattleGameplayManager battleGameplayManager)
            : base(battlefieldPosition, new Character(battleGameplayManager.Parameters.HeroMaxHp, 0), battlefield)
        {
            _battleGameplayManager = battleGameplayManager;
        }

        public override void Suffer(int damage)
        {
            var now = DateTime.Now;

            if ((now - _lastHitTime).TotalSeconds > _battleGameplayManager.Parameters.HeroInvulnerableSeconds)
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
                Character.Regenerate(_battleGameplayManager.Parameters.HeroRegeneration);
        }

        public void Lifesteal()
        {
            if (Character.HP.Value < Character.MaxHP)
                Character.Regenerate(_battleGameplayManager.Parameters.HeroLifestealAmount);
        }
    }
}