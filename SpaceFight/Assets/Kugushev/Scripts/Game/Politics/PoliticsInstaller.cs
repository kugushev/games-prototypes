using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Factories;
using Kugushev.Scripts.Game.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Game.Politics.Factories;
using Kugushev.Scripts.Game.Politics.Interfaces;
using Kugushev.Scripts.Game.Politics.PresentationModels;
using Zenject;

namespace Kugushev.Scripts.Game.Politics
{
    public class PoliticsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPoliticianSelector>().To<ParliamentPresentationModel>().FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<IIntriguesSelector>().To<IntriguesPresentationModel>().FromComponentInHierarchy().AsSingle();

            Container.InstallPrefabFactory<IntrigueCard, IIntriguesPresentationModel, IntrigueCardPresentationModel,
                IntrigueCardPresentationModel.Factory, IntrigueCardFactory>();

            InstallSignals();
        }

        private void InstallSignals()
        {
            Container.InstallTransitiveSignal<CampaignParameters>();
            Container.InstallTransitiveSignal<RevolutionParameters>();
        }
    }
}