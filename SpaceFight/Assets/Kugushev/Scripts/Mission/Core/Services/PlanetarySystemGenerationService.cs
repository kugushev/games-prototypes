using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Core.Specifications;
using Kugushev.Scripts.Mission.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models.Effects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Mission.Core.Services
{
    public class PlanetarySystemGenerationService
    {
        private readonly PlanetarySystemSpecs _specs;
        private readonly PlanetSpecsRegistry _planetSpecsRegistry;
        private readonly Planet.Factory _planetFactory;

        public PlanetarySystemGenerationService(PlanetarySystemSpecs specs,
            PlanetSpecsRegistry planetSpecsRegistry, Planet.Factory planetFactory)
        {
            _specs = specs;
            _planetSpecsRegistry = planetSpecsRegistry;
            _planetFactory = planetFactory;
        }

        public (Sun, IReadOnlyList<Planet>) CreatePlanetarySystemData(MissionInfo info)
        {
            Random.InitState(info.Seed);

            var sun = CreateSun();
            var planets = CreatePlanets(info);
            return (sun, planets);
        }

        private Sun CreateSun()
        {
            var radius = Random.Range(_specs.SunMinRadius, _specs.SunMaxRadius);
            return new Sun(new Position(_specs.Center), radius);
        }

        private IReadOnlyList<Planet> CreatePlanets(MissionInfo missionInfo)
        {
            var result = new List<Planet>();

            int planetsCount = Random.Range(_specs.MinPlanets, _specs.MaxPlanets);

            (int greenHome, int redHome) = GetHomePlanets(planetsCount);
            var homePlanetRule = GetHomePlanetSpec();
            int homeStartDay = Random.Range(0, Orbit.DaysInYear);

            // todo: uncomment
            var oneExtraPlanetForPlayer =
                false; //planetarySystemPerks.TryGetPerks(_specs.PlayerFaction, out var perks) &&
            //perks.GetExtraPlanetOnStart?.Invoke() == true;

            float t = 0f;
            int playerExtraPlanets = oneExtraPlanetForPlayer ? -1 : 0;
            int enemyExtraPlanets = 0;
            for (int i = 0; i < planetsCount; i++)
            {
                var faction = GetFaction(i, greenHome, redHome, missionInfo,
                    ref playerExtraPlanets, ref enemyExtraPlanets);
                t += 1f / planetsCount;

                var startDay = CreateStartDay(homeStartDay, faction);
                var orbit = CreateOrbit(t, startDay);

                var spec = GetPlanetSpec(faction, homePlanetRule, t);

                int production = GetPlanetProduction(spec, faction, missionInfo);
                var power = GetPower(production, missionInfo, faction);

                var planet = _planetFactory.Create(faction, spec.Size, new Production(production), orbit,
                    new Power(power));

                result.Add(planet);
            }

            return result;
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

        private PlanetSpec GetPlanetSpec(Faction faction, PlanetSpec homePlanetSpec, float t)
        {
            switch (faction)
            {
                case Faction.Neutral:
                    IReadOnlyList<PlanetSpec> planetSpecs;
                    if (t < 0.33) planetSpecs = _planetSpecsRegistry.SmallPlanets;
                    else if (t < 0.66) planetSpecs = _planetSpecsRegistry.MediumPlanets;
                    else planetSpecs = _planetSpecsRegistry.BigPlanets;

                    int index = Random.Range(0, planetSpecs.Count);
                    return planetSpecs[index];
                case Faction.Red:
                case Faction.Green:
                    return homePlanetSpec;
                default:
                    throw new ArgumentOutOfRangeException(nameof(faction), faction, null);
            }
        }

        private Faction GetFaction(int i, int greenHome, int redHome, MissionInfo missionInfo,
            ref int playerGotExtraPlanets, ref int enemyGotExtraPlanets)
        {
            var faction = Faction.Neutral;
            if (i == greenHome)
                return Faction.Green;
            if (i == redHome)
                return Faction.Red;

            // todo: refactor this ugly code
            if ((missionInfo.PlayerExtraPlanets ?? 0) > playerGotExtraPlanets)
            {
                if (missionInfo.EnemyExtraPlanets > enemyGotExtraPlanets &&
                    playerGotExtraPlanets > enemyGotExtraPlanets)
                {
                    enemyGotExtraPlanets++;
                    return _specs.PlayerFaction.GetOpposite();
                }

                playerGotExtraPlanets++;
                return _specs.PlayerFaction;
            }

            if (missionInfo.EnemyExtraPlanets > enemyGotExtraPlanets)
            {
                enemyGotExtraPlanets++;
                return _specs.PlayerFaction.GetOpposite();
            }

            return faction;
        }

        private Orbit CreateOrbit(float t, int startDay)
        {
            float radius = Mathf.Lerp(0f, _specs.SystemRadius, t) + _specs.MinDistanceToSun;

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

        private PlanetSpec GetHomePlanetSpec()
        {
            int ruleIndex = Random.Range(0, _planetSpecsRegistry.MediumPlanets.Count);
            return _planetSpecsRegistry.MediumPlanets[ruleIndex];
        }

        private int GetPlanetProduction(PlanetSpec rule, Faction faction, MissionInfo missionInfo)
        {
            var result = Random.Range(rule.MinProduction, rule.MaxProduction);

            if (missionInfo.PlayerHomeProductionMultiplier != null && faction == _specs.PlayerFaction)
                result *= missionInfo.PlayerHomeProductionMultiplier.Value;
            else if (missionInfo.EnemyHomeProductionMultiplier != null && faction.GetOpposite() == _specs.PlayerFaction)
                result *= missionInfo.EnemyHomeProductionMultiplier.Value;

            return result;
        }

        private float GetPower(int production, MissionInfo missionInfo, Faction faction)
        {
            int multiplier = 1;
            if (faction == Faction.Neutral)
                multiplier = GameplayConstants.NeutralStartPowerMultiplier;
            else if (faction == _specs.PlayerFaction && missionInfo.PlayerStartPowerMultiplier != null)
                multiplier = missionInfo.PlayerStartPowerMultiplier.Value;
            else if (faction == _specs.PlayerFaction.GetOpposite() && missionInfo.EnemyStartPowerMultiplier != null)
                multiplier = missionInfo.EnemyStartPowerMultiplier.Value;

            return production * multiplier;
        }
    }
}