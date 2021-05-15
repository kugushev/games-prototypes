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
    internal class PlaygroundState : BaseSceneLoadingState<CampaignModel>
    {
        private readonly IntriguesRepository _intriguesRepository;

        public PlaygroundState(CampaignModel model, IntriguesRepository intriguesRepository)
            : base(model, UnityConstants.PlaygroundScene, true)
        {
            _intriguesRepository = intriguesRepository;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            Model.NextMission = null;
            Model.Playground.Seed = Random.Range(CampaignConstants.MissionSeedMin, CampaignConstants.MissionSeedMax);
            if (Model.LastMissionResult != null)
                if (Model.LastMissionResult.Value.PlayerWins)
                    Model.Playground.PlayerScore++;
                else
                    Model.Playground.AIScore++;
        }

        protected override void OnExitBeforeUnloadScene()
        {
            var playground = Model.Playground;
            Model.NextMission = new MissionInfo(
                seed: playground.Seed,
                difficulty: Difficulty.Normal,
                reward: _intriguesRepository.Stub,
                playerHomeProductionMultiplier: playground.PlayerHomeProductionMultiplier,
                enemyHomeProductionMultiplier: playground.EnemyHomeProductionMultiplier,
                playerExtraPlanets: playground.PlayerExtraPlanets,
                enemyExtraPlanets: playground.EnemyExtraPlanets,
                playerStartPowerMultiplier: playground.PlayerStartPowerMultiplier,
                enemyStartPowerMultiplier: playground.EnemyStartPowerMultiplier
            );
        }
    }
}