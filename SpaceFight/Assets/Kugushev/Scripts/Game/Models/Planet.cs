﻿using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Planet")]
    public class Planet : Model
    {
        [SerializeField] private Faction faction;
        [SerializeField] private PlanetSize size;
        [SerializeField] private int production;
        private readonly TempState _state = new TempState();
        private class TempState
        {
            public int Army;
            public bool Selected;
        }
        
        public Faction Faction => faction;

        public PlanetSize Size => size;

        public int Army => _state.Army;

        public bool Selected
        {
            get => _state.Selected;
            set => _state.Selected = value;
        }

        public UniTask ExecuteProductionCycle()
        {
            _state.Army += production;
            return UniTask.CompletedTask;
        }

        protected override void Dispose(bool destroying)
        {
        }
    }
}