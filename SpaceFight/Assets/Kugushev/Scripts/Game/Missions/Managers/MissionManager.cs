using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Common.Interfaces;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.ValueObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Game.Missions.Managers
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Missions Manager")]
    public class MissionManager : ScriptableObject
    {
        public MissionState? State { get; private set; }
        public Faction? LastWinner { get; private set; }

        public async UniTask NextMission(PlanetarySystem planetarySystem, ConflictParty green, ConflictParty red)
        {
            if (State != null)
            {
                Debug.LogError("Can't start next mission. Current is not finished yet");
                return;
            }

            State = new MissionState(planetarySystem, green, red);
            State.Value.Setup();
            
            LastWinner = null;

            await SceneManager.LoadSceneAsync(UnityConstants.Scenes.MissionScene);
        }

        public async UniTask CheckMissionStatus()
        {
            if (State != null)
            {
                if (IsMissionFinished(State.Value, out var winner))
                {
                    LastWinner = winner;

                    State.Value.Dispose();
                    State = null;

                    await SceneManager.LoadSceneAsync(UnityConstants.Scenes.MissionBriefingScene);
                }
            }
        }

        private static bool IsMissionFinished(MissionState state, out Faction winner)
        {
            winner = Faction.Unspecified;

            bool greedIsAlive = false;
            bool redIsAlive = false;


            foreach (var planet in state.CurrentPlanetarySystem.Planets)
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