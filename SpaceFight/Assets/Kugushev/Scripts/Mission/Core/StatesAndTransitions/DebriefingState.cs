using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Managers;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    public class DebriefingState : BaseSceneLoadingState<MissionModel>
    {
        // private readonly MissionSceneResultPipeline _missionSceneResultPipeline;
        private readonly PerksManager _perksManager;
        private readonly ObjectsPool _objectsPool;
        private readonly List<BasePerk> _achievementsBuffer = new List<BasePerk>(64);

        public DebriefingState(MissionModel model, object missionSceneResultPipeline,
            PerksManager achievementsManager, ObjectsPool objectsPool)
            : base(model, UnityConstants.Scenes.MissionDebriefingScene, true)
        {
            // _missionSceneResultPipeline = missionSceneResultPipeline;
            _perksManager = achievementsManager;
            _objectsPool = objectsPool;
        }

        protected override void AssertModel()
        {
            if (ModelOld.ExecutionResult == null)
                Alert();
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var debriefingInfo = _objectsPool.GetObject<DebriefingSummary, DebriefingSummary.State>(default);

            if (ModelOld.ExecutionResult?.Winner == ModelOld.PlayerFaction)
            {
                _achievementsBuffer.Clear();
                // _perksManager.FindAchieved(_achievementsBuffer, ModelOld.PlayerFaction,
                //     ModelOld.Parameters.PlayerPerksOld);

                debriefingInfo.Fill(_achievementsBuffer);

                _achievementsBuffer.Clear();
            }

            ModelOld.DebriefingSummary = debriefingInfo;
        }

        protected override void OnExitBeforeUnloadScene()
        {
            if (ModelOld.ExecutionResult != null)
            {
                var playerWin = ModelOld.ExecutionResult.Value.Winner switch
                {
                    Faction.Green => true,
                    Faction.Red => false,
                    _ => throw new ArgumentOutOfRangeException(nameof(ModelOld.ExecutionResult.Value.Winner),
                        $"Unexpected winner {ModelOld.ExecutionResult.Value.Winner}")
                };

                if (ModelOld.DebriefingSummary != null)
                {
                    var reward = ModelOld.DebriefingSummary.SelectedAchievement;

                    // _missionSceneResultPipeline.Set(new MissionResult(playerWin, ModelOld.Parameters.MissionInfo,
                    //     reward));
                }
                else
                    Debug.LogError("Reward is null");
            }
            else
                Alert();
        }

        private static void Alert() => Debug.LogError("No execution result found");
    }
}