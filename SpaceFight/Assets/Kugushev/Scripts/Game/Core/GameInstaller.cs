using Kugushev.Scripts.Game.Core.ContextManagement;
using Kugushev.Scripts.Game.Core.Repositories;
using Kugushev.Scripts.Game.Core.Services;
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
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<GameDateStore>().AsSingle();

            InstallContextManagement();

            Container.Bind<ParliamentGenerationService>().AsSingle();
            Container.Bind<PoliticianCharactersRepository>().FromScriptableObject(politicianCharactersRepository)
                .AsSingle();
        }

        private void InstallContextManagement()
        {
            Container.Bind<GameStoreInitializedTransition>().AsSingle();
            Container.Bind<PoliticsState>().AsSingle();
        }
    }
}