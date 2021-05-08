using System;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Core.ContextManagement;
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
            Container.BindInterfacesAndSelfTo<GameDataStore>().AsSingle();
            Container.Bind<Intrigues>().AsSingle();

            InstallContextManagement();

            Container.InstallPoolable<Intrigue, IntrigueCard, IntrigueCard.Factory>();

            Container.Bind<ParliamentGenerationService>().AsSingle();
            Container.Bind<PoliticianCharactersRepository>().FromScriptableObject(politicianCharactersRepository)
                .AsSingle();

            InstallSignals();
        }

        private void InstallContextManagement()
        {
            Container.Bind<GameStoreInitializedTransition>().AsSingle();
            Container.Bind<PoliticsState>().AsSingle();
        }

        private void InstallSignals()
        {
            Container.BindSignal<IntrigueCardObtained>()
                .ToMethod<Intrigues>((intrigues, signal) =>
                {
                    intrigues.HandleCardObtained(signal.CreateCard());
                    if (signal is IDisposable disposable)
                        disposable.Dispose();
                })
                .FromResolve();
            
            // todo: uncommend and fix test
            // Container.InstallSignalAndBind<Intrigue, IntrigueCardObtained, IntrigueCardObtained.Factory, Intrigues>(
            //     (intrigues, signal) => intrigues.HandleCardObtained(signal.CreateCard()));
        }
    }
}