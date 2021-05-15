using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Core.ContextManagement;
using Kugushev.Scripts.Game.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Repositories;
using Kugushev.Scripts.Game.Core.Services;
using Kugushev.Scripts.Game.Core.Signals;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game.Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PoliticianCharactersRepository politicianCharactersRepository = default!;

        private void InstallContextManagement()
        {
            Container.Bind<GameDataInitializedTransition>().AsSingle();
            Container.Bind<PoliticsState>().AsSingle();
            Container.Bind<CampaignState>().AsSingle();
            Container.Bind<RevolutionState>().AsSingle();

            Container.InstallSignaledTransition<CampaignParameters>();
            Container.InstallExitState<CampaignExitParameters>();
            Container.InstallSignaledTransition<RevolutionParameters>();
            Container.InstallSignaledTransition<GameExitParameters>();
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameDataInitializer>().AsSingle();

            InstallModels();
            InstallContextManagement();
            InstallServices();
            InstallSignals();
        }

        private void InstallModels()
        {
            Container.Bind<Parliament>().AsSingle();
            Container.Bind<Intrigues>().AsSingle();
            Container.Bind<IIntrigues>().To<Intrigues>().FromResolve();

            Container.InstallPoolable<Intrigue, IntrigueCard, IntrigueCard.Factory>();
        }

        private void InstallServices()
        {
            Container.Bind<ParliamentGenerationService>().AsSingle();
            Container.Bind<PoliticianCharactersRepository>().FromScriptableObject(politicianCharactersRepository)
                .AsSingle();
        }

        private void InstallSignals()
        {
            Container.InstallSignalAndBind
                <(IntrigueCard, IPolitician), ApplyIntrigueCard, ApplyIntrigueCard.Factory, Intrigues>(
                    (intrigues, signal) =>
                    {
                        signal.ExecuteApply();
                        intrigues.HandleCardApplied(signal.Card);
                    });
        }
    }
}