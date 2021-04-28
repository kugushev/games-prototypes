using Kugushev.Scripts.Game.ContextManagement;
using Kugushev.Scripts.Game.Repositories;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Game.Widgets;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PoliticianCharactersRepository politicianCharactersRepository = default!;

        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<GameDateStore>().AsSingle();

            InstallContextManagement();

            Container.Bind<ParliamentGenerationService>().AsSingle();
            Container.Bind<PoliticianCharactersRepository>().FromScriptableObject(politicianCharactersRepository)
                .AsSingle();

            InstallPresentation();
        }

        private void InstallPresentation()
        {
            Container.Bind<ParliamentWidget>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallContextManagement()
        {
            Container.Bind<GameStoreInitializedTransition>().AsSingle();
            Container.Bind<PoliticsState>().AsSingle();
        }
    }
}