using Kugushev.Scripts.App.Core.ContextManagement;
using Kugushev.Scripts.Common.ContextManagement;
using Zenject;

namespace Kugushev.Scripts.App.Core
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AbstractContextManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<MainMenuState>().AsSingle();
            Container.Bind<GameState>().AsSingle();
        }
    }
}