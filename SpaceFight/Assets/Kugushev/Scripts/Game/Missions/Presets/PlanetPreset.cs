using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.Presets
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Planet")]
    public class PlanetPreset : ScriptableObject
    {
        [SerializeField] private Faction faction;
        [SerializeField] private PlanetSize size;
        [SerializeField] private int production;
        [SerializeField] private Vector3 position;

        // public Planet ToPlanet(ObjectsPool pool, Sun sun)
        // {
        //     return pool.GetObject<Planet, Planet.State>(
        //         new Planet.State(faction, size, production, new Orbit(), sun));
        // }
    }
}