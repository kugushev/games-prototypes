using System;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Entities;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Mission.ProceduralGeneration
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Planetary System Generator")]
    public class PlanetarySystemGenerator : ScriptableObject
    {
        [SerializeField] private ObjectsPool objectsPool;
        [SerializeField] private MissionManager missionManager;

        [Header("Rules")] [SerializeField] private float systemRadius = 0.8f;
        [SerializeField] private Vector3 center = new Vector3(0f, 1.5f, 0.5f);
        [SerializeField] private float sunMaxRadius = 0.05f;
        [SerializeField] private float sunMinRadius = 0.2f;
        [SerializeField] private int minPlanets = 3;
        [SerializeField] private int maxPlanets = 6;
        [SerializeField] private PlanetRule[] planetRules;

        public PlanetarySystem CreatePlanetarySystem(int seed)
        {
            Random.InitState(seed);

            var sun = CreateSun();
            var result = objectsPool.GetObject<PlanetarySystem, PlanetarySystem.State>(new PlanetarySystem.State(sun));
            AddPlanets(result, sun);
            return result;
        }

        private Sun CreateSun()
        {
            var radius = Random.Range(sunMinRadius, sunMaxRadius);
            return new Sun(new Position(center), radius);
        }

        private void AddPlanets(PlanetarySystem planetarySystem, Sun sun)
        {
            int planetsCount = Random.Range(minPlanets, maxPlanets);

            (int greenHome, int redHome) = GetHomePlanets(planetsCount);

            float t = 0f;
            for (int i = 0; i < planetsCount; i++)
            {
                t += 1f / planetsCount;
                var orbit = CreateOrbit(t);

                var faction = GetFaction(i, greenHome, redHome);
                var rule = GetPlanetRule(faction);

                int production = Random.Range(rule.MinProduction, rule.MaxProduction);

                var planet = objectsPool.GetObject<Planet, Planet.State>(
                    new Planet.State(faction, rule.Size, production, orbit, sun, missionManager.EventsManager));

                planetarySystem.AddPlanet(planet);
            }
        }

        private PlanetRule GetPlanetRule(Faction faction)
        {
            switch (faction)
            {
                case Faction.Neutral:
                    int ruleIndex = Random.Range(0, planetRules.Length);
                    return planetRules[ruleIndex];
                case Faction.Red:
                case Faction.Green:
                    return GetSidesRule();
                default:
                    throw new ArgumentOutOfRangeException(nameof(faction), faction, null);
            }
        }

        private static Faction GetFaction(int i, int greenHome, int redHome)
        {
            var faction = Faction.Neutral;
            if (i == greenHome)
                faction = Faction.Green;
            if (i == redHome) faction = Faction.Red;
            return faction;
        }

        private Orbit CreateOrbit(float t)
        {
            float radius = Mathf.Lerp(0f, systemRadius, t);
            return new Orbit(radius, Random.rotation);
        }

        private (int green, int red) GetHomePlanets(int planetsCount)
        {
            var green = Random.Range(0, planetsCount);
            var red = green;
            while (red == green)
                red = Random.Range(0, planetsCount);
            return (green, red);
        }

        private PlanetRule GetSidesRule()
        {
            PlanetRule rule;
            do
            {
                int ruleIndex = Random.Range(0, planetRules.Length);
                rule = planetRules[ruleIndex];
            } while (rule.Size < PlanetSize.Earth);

            return rule;
        }
    }
}