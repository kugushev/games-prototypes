using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    [CreateAssetMenu(menuName = "Game/Planet")]
    public class Planet: Model
    {
        [SerializeField] private Faction faction;
        [SerializeField] private PlanetSize size;

        public Faction Faction => faction;

        public PlanetSize Size => size;

        protected override void Dispose(bool destroying)
        {
            
        }
    }
}