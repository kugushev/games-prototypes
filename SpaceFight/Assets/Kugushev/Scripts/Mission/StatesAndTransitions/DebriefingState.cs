using System;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using UnityEngine;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    public class DebriefingState : BaseSceneLoadingState<MissionModel>
    {
        private readonly MissionSceneResultPipeline _missionSceneResultPipeline;

        public DebriefingState(MissionModel model, MissionSceneResultPipeline missionSceneResultPipeline)
            : base(model, UnityConstants.Scenes.MissionDebriefingScene, true)
        {
            _missionSceneResultPipeline = missionSceneResultPipeline;
        }

        protected override void OnExitBeforeUnloadScene()
        {
            if (Model.ExecutionResult != null)
            {
                var playerWin = Model.ExecutionResult.Value.Winner switch
                {
                    Faction.Green => true,
                    Faction.Red => false,
                    _ => throw new ArgumentOutOfRangeException(nameof(Model.ExecutionResult.Value.Winner),
                        $"Unexpected winner {Model.ExecutionResult.Value.Winner}")
                };
                _missionSceneResultPipeline.Set(new MissionResult(playerWin));
            }
            else
                Debug.LogError("No Execution Result found");
        }
    }
}