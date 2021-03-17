using System;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects.PlayerProperties;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Mission.ProceduralGeneration
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Planetary System Generator")]
    public class PlanetarySystemGenerator : ScriptableObject
    {
        [SerializeField] private ObjectsPool objectsPool;
        [SerializeField] private MissionEventsCollector eventsCollector;

        [Header("Rules")] [SerializeField] private float systemRadius = 0.9f;
        [SerializeField] private float minDistanceToSun = 0.1f;
        [SerializeField] private Vector3 center = new Vector3(0f, 1.5f, 0.5f);
        [SerializeField] private float sunMinRadius = 0.05f;
        [SerializeField] private float sunMaxRadius = 0.15f;
        [SerializeField] private int minPlanets = 3;
        [SerializeField] private int maxPlanets = 6;
        [SerializeField] private PlanetRule[] smallPlanetsRules;
        [SerializeField] private PlanetRule[] mediumPlanetsRules;
        [SerializeField] private PlanetRule[] bigPlanetsRules;

        public PlanetarySystem CreatePlanetarySystem(MissionProperties properties, Faction playerFaction,
            PlanetarySystemProperties planetarySystemProperties)
        {
            Random.InitState(properties.Seed);

            var sun = CreateSun();
            var result = objectsPool.GetObject<PlanetarySystem, PlanetarySystem.State>(new PlanetarySystem.State(sun));
            AddPlanets(result, sun, planetarySystemProperties, properties, playerFaction);
            return result;
        }

        private Sun CreateSun()
        {
            var radius = Random.Range(sunMinRadius, sunMaxRadius);
            return new Sun(new Position(center), radius);
        }

        private void AddPlanets(PlanetarySystem planetarySystem, Sun sun,
            PlanetarySystemProperties planetarySystemProperties, MissionProperties missionProperties,
            Faction playerFaction)
        {
            int planetsCount = Random.Range(minPlanets, maxPlanets);

            (int greenHome, int redHome) = GetHomePlanets(planetsCount);
            var homePlanetRule = GetHomePlanetRule();
            int homeStartDay = Random.Range(0, Orbit.DaysInYear);

            float t = 0f;
            for (int i = 0; i < planetsCount; i++)
            {
                var faction = GetFaction(i, greenHome, redHome);
                t += 1f / planetsCount;

                var startDay = CreateStartDay(homeStartDay, faction);
                var orbit = CreateOrbit(t, startDay);

                var rule = GetPlanetRule(faction, homePlanetRule, t);

                int production = GetPlanetProduction(rule, faction, missionProperties, playerFaction);

                var planet = objectsPool.GetObject<Planet, Planet.State>(new Planet.State(
                    faction, rule.Size, production, orbit, sun, eventsCollector, planetarySystemProperties));

                planetarySystem.AddPlanet(planet);
            }
        }


        private int CreateStartDay(int homeStartDay, Faction faction)
        {
            switch (faction)
            {
                case Faction.Green:
                    return homeStartDay;
                case Faction.Red:
                    return Orbit.DaysInYear - homeStartDay; // opposite to green
                default:
                    return Random.Range(0, Orbit.DaysInYear);
            }
        }

        private PlanetRule GetPlanetRule(Faction faction, PlanetRule homePlanetRule, float t)
        {
            switch (faction)
            {
                case Faction.Neutral:
                    PlanetRule[] rules;
                    if (t < 0.33) rules = smallPlanetsRules;
                    else if (t < 0.66) rules = mediumPlanetsRules;
                    else rules = bigPlanetsRules;

                    int ruleIndex = Random.Range(0, rules.Length);
                    return rules[ruleIndex];
                case Faction.Red:
                case Faction.Green:
                    return homePlanetRule;
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

        private Orbit CreateOrbit(float t, int startDay)
        {
            float radius = Mathf.Lerp(0f, systemRadius, t) + minDistanceToSun;

            float alphaVariation = (1f - t) * 180;
            float alpha = Random.Range(-alphaVariation, alphaVariation);

            // should be smaller to avoid touching player belly
            float betaVariation = alphaVariation / 2;
            float beta = Random.Range(-betaVariation, betaVariation);

            return new Orbit(radius, new Degree(alpha), new Degree(beta), startDay);
        }

        private (int green, int red) GetHomePlanets(int planetsCount)
        {
            var green = Random.Range(0, planetsCount);
            var red = green;
            while (red == green)
                red = Random.Range(0, planetsCount);
            return (green, red);
        }

        private PlanetRule GetHomePlanetRule()
        {
            int ruleIndex = Random.Range(0, mediumPlanetsRules.Length);
            return mediumPlanetsRules[ruleIndex];
        }

        private static int GetPlanetProduction(PlanetRule rule, Faction faction, MissionProperties missionProperties,
            Faction playerFaction)
        {
            var result = Random.Range(rule.MinProduction, rule.MaxProduction);

            if (missionProperties.PlayerHomeProductionMultiplier != null && faction == playerFaction)
                result *= missionProperties.PlayerHomeProductionMultiplier.Value;
            else if (missionProperties.EnemyHomeProductionMultiplier != null && faction.GetOpposite() == playerFaction)
                result *= missionProperties.EnemyHomeProductionMultiplier.Value;

            return result;
        }
    }
}