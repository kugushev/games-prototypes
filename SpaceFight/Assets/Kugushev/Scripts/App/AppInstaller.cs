using Kugushev.Scripts.App.ContextManagement;
using Kugushev.Scripts.Common.ContextManagement;
using Zenject;

namespace Kugushev.Scripts.App
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AbstractContextManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<MainMenuState>().AsSingle();
            Container.Bind<GameModeState>().AsSingle();
        }
    }
}