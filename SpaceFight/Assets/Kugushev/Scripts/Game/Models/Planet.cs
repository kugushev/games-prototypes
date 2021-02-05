using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models.Abstractions;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Planet")]
    public class Planet : Model, IFighter
    {
        [SerializeField] private Faction faction;
        [SerializeField] private PlanetSize size;
        [SerializeField] private int production;
        private readonly TempState _state = new TempState();

        private class TempState
        {
            public int Power;
            public bool Selected;
            public Faction CurrentFaction;
        }

        public Faction Faction => _state.CurrentFaction == Faction.Unspecified 
            ? _state.CurrentFaction = faction
            : _state.CurrentFaction;

        public PlanetSize Size => size;

        public int Power => _state.Power;

        public bool Selected
        {
            get => _state.Selected;
            set => _state.Selected = value;
        }

        public UniTask ExecuteProductionCycle()
        {
            _state.Power += production;
            return UniTask.CompletedTask;
        }

        public int Recruit()
        {
            if (_state.Power <= GameConstants.SoftCapArmyPower)
            {
                var allPower = _state.Power;
                _state.Power = 0;
                return allPower;
            }

            _state.Power -= GameConstants.SoftCapArmyPower;
            return GameConstants.SoftCapArmyPower;
        }

        public void Reinforce(Army army)
        {
            _state.Power += army.Power;
        }
        
        public bool TryCapture(Army invader)
        {
            _state.Power -= GameConstants.UnifiedDamage;
            
            if (_state.Power < 0)
            {
                _state.Power *= -1;
                _state.CurrentFaction = invader.Faction;
                return true;
            }

            return false;
        }
        
        protected override void Dispose(bool destroying)
        {
        }
    }
}