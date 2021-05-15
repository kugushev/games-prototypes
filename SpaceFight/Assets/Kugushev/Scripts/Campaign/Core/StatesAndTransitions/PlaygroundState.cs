using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Repositories;
using Kugushev.Scripts.Game.Core.Services;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class PlaygroundState : BaseSceneLoadingState<CampaignModelOld>
    {
        private readonly IntriguesRepository _intriguesRepository;

        public PlaygroundState(CampaignModelOld modelOld, IntriguesRepository intriguesRepository)
            : base(modelOld, UnityConstants.PlaygroundScene, true)
        {
            _intriguesRepository = intriguesRepository;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            ModelOld.NextMission = null;
            // if (ModelOld.LastMissionResult != null)
            //     if (ModelOld.LastMissionResult.Value.PlayerWins)
            //         ModelOld.Playground.PlayerScore++;
            //     else
            //         ModelOld.Playground.AIScore++;
        }

        protected override void OnExitBeforeUnloadScene()
        {
            // // var playground = ModelOld.Playground;
            // ModelOld.NextMission = new MissionInfo(
            //     seed: playground.Seed,
            //     difficulty: Difficulty.Normal,
            //     reward: _intriguesRepository.Stub,
            //     playerHomeProductionMultiplier: playground.PlayerHomeProductionMultiplier,
            //     enemyHomeProductionMultiplier: playground.EnemyHomeProductionMultiplier,
            //     playerExtraPlanets: playground.PlayerExtraPlanets,
            //     enemyExtraPlanets: playground.EnemyExtraPlanets,
            //     playerStartPowerMultiplier: playground.PlayerStartPowerMultiplier,
            //     enemyStartPowerMultiplier: playground.EnemyStartPowerMultiplier
            // );
        }
    }
}