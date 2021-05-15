using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Campaign.MissionSelection.Factories;
using Kugushev.Scripts.Campaign.MissionSelection.Interfaces;
using Kugushev.Scripts.Campaign.MissionSelection.PresentationModels;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Factories;
using Kugushev.Scripts.Game.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Campaign.MissionSelection
{
    public class MissionSelectionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.InstallPrefabFactory<MissionInfo,
                IMissionSelectionPresentationModel,
                MissionCardPresentationModel,
                MissionCardPresentationModel.Factory,
                MissionCardFactory>();

            InstallSignals();
        }

        private void InstallSignals()
        {
            Container.InstallTransitiveSignal<CampaignExitParameters>();
        }
    }
}