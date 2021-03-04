﻿using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Missions.AI.Tactical;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Interfaces;
using Kugushev.Scripts.Game.Missions.Managers;
using Kugushev.Scripts.Game.Missions.ProceduralGeneration;
using Kugushev.Scripts.Game.Missions.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Controllers
{
    public class TestMissionController : MonoBehaviour
    {
        [SerializeField] private MissionManager missionManager;
        
        [Header("Planetary System")]
        [SerializeField] private PlanetarySystemGenerator planetarySystemGenerator;

        [Header("Mission Related Assets")]
        [SerializeField] private SimpleAI greenAi;
        [SerializeField] private SimpleAI redAi;
        [SerializeField] private Fleet greenFleet;
        [SerializeField] private Fleet redFleet;

        public static bool MissionFinished { get; private set; }
        public static int Seed { get; set; }

        private void Start()
        {
            StartCoroutine(RunMission().ToCoroutine());
        }

        private async UniTask RunMission()
        {
            MissionFinished = false;
            missionManager.MissionFinished += _ =>
            {
                MissionFinished = true;
                return true;
            };

            var planetarySystem = planetarySystemGenerator.CreatePlanetarySystem(Seed);
            
            var green = new ConflictParty(Faction.Green, greenFleet, greenAi);
            var red = new ConflictParty(Faction.Red, redFleet, redAi);

            await missionManager.NextMission(planetarySystem, green, red, false);
        }
    }
}