using System;
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
        // use for interaction to the old architecture
        [SerializeField] private PoliticianCharactersRepository politicianCharactersRepository = default!;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameDataInitializer>().AsSingle();

            Container.Bind<Parliament>().AsSingle();

            Container.Bind<Intrigues>().AsSingle();
            Container.Bind<IIntrigues>().To<Intrigues>().FromResolve();

            InstallContextManagement();

            Container.InstallPoolable<Intrigue, IntrigueCard, IntrigueCard.Factory>();

            Container.Bind<ParliamentGenerationService>().AsSingle();
            Container.Bind<PoliticianCharactersRepository>().FromScriptableObject(politicianCharactersRepository)
                .AsSingle();

            InstallSignals();
        }

        private void InstallContextManagement()
        {
            Container.Bind<GameDataInitializedTransition>().AsSingle();
            Container.Bind<PoliticsState>().AsSingle();
            Container.Bind<CampaignState>().AsSingle();
            Container.Bind<RevolutionState>().AsSingle();
        }

        private void InstallSignals()
        {
            Container.InstallSignalAndBind<Intrigue, ObtainIntrigueCard, ObtainIntrigueCard.Factory, Intrigues>(
                (intrigues, signal) => intrigues.ObtainCard(signal.CreateCard()));

            Container.InstallSignalAndBind
                <(IntrigueCard, IPolitician), ApplyIntrigueCard, ApplyIntrigueCard.Factory, Intrigues>(
                    (intrigues, signal) =>
                    {
                        signal.ExecuteApply();
                        intrigues.HandleCardApplied(signal.Card);
                    });

            Container.InstallTransitiveSignal<CampaignParameters>();
            Container.InstallTransitiveSignal<RevolutionParameters>();
        }
    }
}