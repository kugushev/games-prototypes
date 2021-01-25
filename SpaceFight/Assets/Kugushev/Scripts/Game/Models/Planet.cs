using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    [CreateAssetMenu(menuName = "Game/Planet")]
    public class Planet : Model
    {
        [SerializeField] private Faction faction;
        [SerializeField] private PlanetSize size;
        [SerializeField] private int production;
        [SerializeField] private int army;

        private void Awake()
        {
            army = 0;
        }

        public Faction Faction => faction;

        public PlanetSize Size => size;

        public int Army => army;
        
        public bool Selected { get; set; }

        public UniTask ExecuteProductionCycle()
        {
            army += production;
            return UniTask.CompletedTask;
        }

        protected override void Dispose(bool destroying)
        {
        }
    }
}