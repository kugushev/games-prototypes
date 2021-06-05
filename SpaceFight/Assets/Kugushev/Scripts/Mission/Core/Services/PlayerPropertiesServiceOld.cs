﻿using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.Utils.ValuesProcessing;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Core.Specifications;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Services
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(PlayerPropertiesServiceOld))]
    public class PlayerPropertiesServiceOld : ScriptableObject
    {
        [SerializeField] private PerksRegistry? achievementsManager;
        [SerializeField] private ObjectsPool? objectsPool;

        private readonly List<BasePerk> _achievementBuffer = new List<BasePerk>(128);

        public static FleetPerks.State CreateDefaultFleetPerksState(ObjectsPool objectsPool)
        {
            return new FleetPerks.State(
                objectsPool.GetObject<ValuePipelineOld<ArmyOld>, int>(0),
                objectsPool.GetObject<ValuePipelineOld<ArmyOld>, int>(0),
                objectsPool.GetObject<ValuePipelineOld<ArmyOld>, int>(0),
                objectsPool.GetObject<ValuePipelineOld<(Planet target, Faction playerFaction)>, int>(0)
            );
        }

        public (PlanetarySystemPerks, FleetPerks) GetPlayerProperties(Faction playerFaction,
            MissionParameters parameters)
        {
            Asserting.NotNull(achievementsManager, objectsPool);

            _achievementBuffer.Clear();

            // achievementsManager.FindMatched(_achievementBuffer, parameters.PlayerPerksOld);

            var fleetPerksBuilder = CreateDefaultFleetPerksState(objectsPool);
            var planetarySystemPerksBuilder = new PlanetarySystemPerks.State(playerFaction,
                objectsPool.GetObject<ValuePipelineOld<Planet>, int>(0),
                objectsPool.GetObject<ValuePipelineOld<Planet>, int>(0));

            // foreach (var achievement in _achievementBuffer)
            //     achievement.Apply(ref fleetPerksBuilder, ref planetarySystemPerksBuilder);

            _achievementBuffer.Clear();

            var fleetPerks = objectsPool.GetObject<FleetPerks, FleetPerks.State>(fleetPerksBuilder);
            var planetarySystemPerks = objectsPool.GetObject<PlanetarySystemPerks, PlanetarySystemPerks.State>(
                planetarySystemPerksBuilder);

            return (planetarySystemPerks, fleetPerks);
        }
    }
}