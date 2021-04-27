using Kugushev.Scripts.Game.ContextManagement;
using Kugushev.Scripts.Game.Services;
using Zenject;

namespace Kugushev.Scripts.Game
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<GameDateStore>().AsSingle();

            InstallContextManagement();

            Container.Bind<ParliamentGenerationService>().AsSingle();
        }

        private void InstallContextManagement()
        {
            Container.Bind<GameStoreInitializedTransition>().AsSingle();
            Container.Bind<PoliticsState>().AsSingle();
        }
    }
}