using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Entities;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Game.Managers
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Missions Manager")]
    public class MissionsManager : ScriptableObject
    {
        public PlanetarySystem CurrentPlanetarySystem { get; private set; }
        public IReadOnlyList<IAIAgent> AIAgents { get; private set; }
        public Faction? LastWinner { get; private set; }

        public async UniTask NextMission(PlanetarySystem planetarySystem, IReadOnlyList<IAIAgent> aiAgents)
        {
            if (CurrentPlanetarySystem != null || AIAgents != null)
            {
                Debug.LogError("Can't start next mission. Current is not finished yet");
                return;
            }

            CurrentPlanetarySystem = planetarySystem;
            AIAgents = aiAgents;
            LastWinner = null;
            
            await SceneManager.LoadSceneAsync(UnityConstants.Scenes.MissionScene);
        }

        public async UniTask CheckMissionStatus()
        {
            if (CurrentPlanetarySystem != null)
            {
                if (IsMissionFinished(out var winner))
                {
                    LastWinner = winner;
                    
                    // todo: clenup fleet 
                    CurrentPlanetarySystem = null;
                    AIAgents = null;
                    await SceneManager.LoadSceneAsync(UnityConstants.Scenes.MissionBriefingScene);
                }
            }
        }

        private bool IsMissionFinished(out Faction winner)
        {
            winner = Faction.Unspecified;
            bool greedIsAlive = false;
            bool redIsAlive = false;

            foreach (var planet in CurrentPlanetarySystem.Planets)
            {
                switch (planet.Faction)
                {
                    case Faction.Green:
                        greedIsAlive = true;
                        break;
                    case Faction.Red:
                        redIsAlive = true;
                        break;
                }

                if (greedIsAlive && redIsAlive)
                    return false;
            }

            winner = (greedIsAlive, redIsAlive) switch
            {
                (true, false) => Faction.Green,
                (false, true) => Faction.Red,
                (false, false) => Faction.Neutral,
                _ => Faction.Unspecified
            };
            return true;
        }
    }
}