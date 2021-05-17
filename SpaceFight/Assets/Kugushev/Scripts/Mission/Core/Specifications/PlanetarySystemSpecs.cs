using Kugushev.Scripts.Mission.Enums;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Core.Specifications
{
    public class PlanetarySystemSpecs
    {
        public virtual Faction PlayerFaction { get; } = Faction.Green;
        public virtual float SystemRadius { get; } = 0.9f;
        public virtual float MinDistanceToSun { get; } = 0.1f;
        public virtual Vector3 Center { get; } = new Vector3(0f, 1.5f, 0.5f);
        public virtual float SunMinRadius { get; } = 0.05f;
        public virtual float SunMaxRadius { get; } = 0.15f;
        public virtual int MinPlanets { get; } = 3;
        public virtual int MaxPlanets { get; } = 6;
    }
}