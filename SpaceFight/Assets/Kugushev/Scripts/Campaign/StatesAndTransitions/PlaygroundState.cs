using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Enums;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class PlaygroundState : BaseSceneLoadingState<CampaignModel>
    {
        public PlaygroundState(CampaignModel model) : base(model, UnityConstants.PlaygroundScene, true)
        {
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