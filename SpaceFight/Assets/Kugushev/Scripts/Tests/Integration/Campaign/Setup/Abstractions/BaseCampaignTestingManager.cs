using System;
using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Tests.Integration.Campaign.Setup.Abstractions
{
    internal abstract class BaseCampaignTestingManager : BaseManager<CampaignModel>
    {
        [SerializeField] private ObjectsPool objectsPool;
        [SerializeField] private CampaignModelProvider modelProvider;

        public static int? Seed { get; set; }

        protected override CampaignModel InitRootModel()
        {
            var campaignInfo = new CampaignInfo(Seed ?? DateTime.UtcNow.Millisecond,
                null, PerkIdHelper.AllPerks, false, true);

            var model = objectsPool.GetObject<CampaignModel, CampaignModel.State>(new CampaignModel.State(campaignInfo,
                objectsPool.GetObject<MissionSelection, MissionSelection.State>(
                    new MissionSelection.State(CampaignConstants.MaxBudget)),
                objectsPool.GetObject<Playground, Playground.State>(new Playground.State()),
                objectsPool.GetObject<PlayerPerks, PlayerPerks.State>(new PlayerPerks.State()),
                objectsPool.GetObject<CampaignResult, CampaignResult.State>(new CampaignResult.State())
            ));

            modelProvider.Set(model);

            return model;
        }

        protected override void OnStart()
        {
            Random.InitState(RootModel.CampaignInfo.Seed);
        }

        protected override void Dispose()
        {
            modelProvider.Cleanup();
        }
    }
}