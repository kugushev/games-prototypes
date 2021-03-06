using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Entities;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Kugushev.Scripts.Mission.Constants.UnityConstants.Scenes;

namespace Kugushev.Scripts.Mission.Managers
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Missions Manager")]
    public class MissionManager : ScriptableObject
    {
        [SerializeField] private MissionEventsManager eventsManager;

        public MissionState? State { get; private set; }
        public Faction? LastWinner { get; private set; }

        public int Wins { get; private set; }
        public int Looses { get; private set; }

        public event Func<Faction, bool> MissionFinished;

        public MissionEventsManager EventsManager => eventsManager;

        public async UniTask NextMission(PlanetarySystem planetarySystem, ConflictParty green, ConflictParty red,
            bool preparation)
        {
            if (State != null)
            {
                Debug.LogError("Can't start next mission. Current is not finished yet");
                return;
            }

            State = new MissionState(planetarySystem, green, red);
            State.Value.Setup();

            LastWinner = null;

            var scene = preparation ? MissionPreparationScene : MissionScene;
            await SceneManager.LoadSceneAsync(scene);
        }

        public async UniTask StartMission()
        {
            if (State == null)
            {
                Debug.LogError("Mission is not prepared yet");
                return;
            }

            await SceneManager.LoadSceneAsync(MissionScene);
        }

        public async UniTask CheckMissionStatus()
        {
            if (State != null)
            {
                if (IsMissionFinished(State.Value, out var winner))
                {
                    LastWinner = winner;
                    switch (winner)
                    {
                        case Faction.Green:
                            Wins++;
                            break;
                        case Faction.Red:
                            Looses++;
                            break;
                    }


                    State.Value.Dispose();
                    State = null;

                    bool? shouldSuspend = MissionFinished?.Invoke(winner);

                    if (shouldSuspend != true)
                        await SceneManager.LoadSceneAsync(MissionBriefingScene);
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