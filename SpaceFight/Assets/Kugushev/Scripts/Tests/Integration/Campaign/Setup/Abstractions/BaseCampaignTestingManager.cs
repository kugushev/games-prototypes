using System;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Tests.Integration.Campaign.Setup.Abstractions
{
    internal abstract class BaseCampaignTestingManager : BaseManager<CampaignModel>
    {
        [SerializeField] private CampaignModelProvider modelProvider;

        public static int? Seed { get; set; }

        protected override CampaignModel InitRootModel()
        {
            var campaignInfo = new CampaignInfo(Seed ?? DateTime.UtcNow.Millisecond, false);

            var model = new CampaignModel(campaignInfo);
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