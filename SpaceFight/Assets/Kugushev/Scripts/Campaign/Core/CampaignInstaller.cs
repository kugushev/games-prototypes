using Kugushev.Scripts.Campaign.Core.ContextManagement;
using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Core.Services;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Game.Core.Repositories;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core
{
    public class CampaignInstaller : MonoInstaller
    {
        [SerializeField] private IntriguesRepository intriguesRepository = default!;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CampaignDataInitializer>().AsSingle();

            InstallModels();
            InstallContextManagement();
            InstallServices();
            InstallSignals();
        }

        private void InstallModels()
        {
            Container.Bind<CampaignMissions>().AsSingle();
            Container.Bind<ICampaignMissions>().To<CampaignMissions>().FromResolve();

            Container.Bind<PlayerPerks>().AsSingle();
            Container.Bind<IPlayerPerks>().To<PlayerPerks>().FromResolve();
        }

        private void InstallContextManagement()
        {
            Container.Bind<MissionSelectionState>().AsSingle();
            Container.Bind<MissionState>().AsSingle();

            Container.InstallSignaledTransition<MissionParameters>();
            Container.InstallSignaledTransition<CampaignExitParameters>();
        }

        private void InstallServices()
        {
            Container.Bind<MissionsGenerationService>().AsSingle();
            Container.Bind<IntriguesRepository>().FromScriptableObject(intriguesRepository).AsSingle();
        }

        private void InstallSignals()
        {
            Container.InstallTransitiveSignal<MissionParameters, CampaignMissions>(
                (cm, signal) => cm.OnMissionSelected());

            Container.InstallTransitiveSignal<MissionExitParameters, CampaignMissions>(
                (cm, signal) => cm.OnMissionFinished(signal.Parameters));
        }
    }
}