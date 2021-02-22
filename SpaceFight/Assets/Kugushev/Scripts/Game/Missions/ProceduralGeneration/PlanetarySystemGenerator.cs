using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.Missions.ProceduralGeneration
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Planetary System Generator")]
    public class PlanetarySystemGenerator : ScriptableObject
    {
        [SerializeField] private ObjectsPool objectsPool;

        [Header("Rules")] [SerializeField] private float systemRadius = 2f;
        [SerializeField] private Vector3 center = new Vector3(0f, 1.5f, 0.5f);
        [SerializeField] private float sunMaxRadius = 0.05f;
        [SerializeField] private float sunMinRadius = 0.2f;
        [SerializeField] private int minPlanets = 3;
        [SerializeField] private int maxPlanets = 10;
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
            PlanetRule sidesRule = null;
            

            float t = 0f;
            for (int i = 0; i < planetsCount; i++)
            {
                t += 1f / planetsCount;
                var orbit = CreateOrbit(t);

                int ruleIndex = Random.Range(0, planetRules.Length);

                var faction = GetFaction(i, greenHome, redHome);

                var rule = planetRules[ruleIndex];
                
                sidesRule = ApplySamePlanetRule(faction, sidesRule, ref rule);

                int production = Random.Range(rule.MinProduction, rule.MaxProduction);

                var planet = objectsPool.GetObject<Planet, Planet.State>(
                    new Planet.State(faction, rule.Size, production, orbit, sun));

                planetarySystem.AddPlanet(planet);
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

        private static PlanetRule ApplySamePlanetRule(Faction faction, PlanetRule sidesRule, ref PlanetRule rule)
        {
            if (faction != Faction.Neutral)
            {
                if (sidesRule == null)
                    sidesRule = rule;
                else
                    rule = sidesRule;
            }

            return sidesRule;
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
    }
}