﻿using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Presentation.Factories;
using Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels;
using Kugushev.Scripts.Common.Core.ZenjectUtils;
using Zenject;

namespace Kugushev.Scripts.Campaign.Presentation
{
    public class CampaignPresentationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.InstallPrefabFactory<City, CityRPM, CityRPM.Factory, CityFactory>();
        }
    }
}