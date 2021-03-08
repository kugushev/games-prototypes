﻿using System;
using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Managers;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    public class DebriefingState : BaseSceneLoadingState<MissionModel>
    {
        private readonly MissionSceneResultPipeline _missionSceneResultPipeline;
        private readonly AchievementsManager _achievementsManager;
        private readonly ObjectsPool _objectsPool;
        private readonly Faction _playerFaction;
        private readonly List<AbstractAchievement> _achievementsBuffer = new List<AbstractAchievement>(64);

        public DebriefingState(MissionModel model, MissionSceneResultPipeline missionSceneResultPipeline,
            AchievementsManager achievementsManager, ObjectsPool objectsPool, Faction playerFaction)
            : base(model, UnityConstants.Scenes.MissionDebriefingScene, true)
        {
            _missionSceneResultPipeline = missionSceneResultPipeline;
            _achievementsManager = achievementsManager;
            _objectsPool = objectsPool;
            _playerFaction = playerFaction;
        }

        protected override void AssertModel()
        {
            if (Model.ExecutionResult == null)
                Alert();
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var debriefingInfo = _objectsPool.GetObject<DebriefingInfo, DebriefingInfo.State>(default);

            if (Model.ExecutionResult?.Winner == _playerFaction)
            {
                _achievementsBuffer.Clear();
                _achievementsManager.FindSuitableAchievements(_achievementsBuffer, _playerFaction);
                
                debriefingInfo.Fill(_achievementsBuffer);
                
                _achievementsBuffer.Clear();
            }

            Model.DebriefingInfo = debriefingInfo;

            base.OnEnterBeforeLoadScene();
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
                Alert();
        }

        private static void Alert() => Debug.LogError("No execution result found");
    }
}