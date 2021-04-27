using Kugushev.Scripts.App.Modes;
using Kugushev.Scripts.Common.Modes;
using Zenject;

namespace Kugushev.Scripts.App
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AppManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<AppSceneLoader>().AsSingle();

            // todo: make it POCO
            InstallAppMode();
        }

        private void InstallAppMode()
        {
            Container.Bind<AbstractModeManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<MainMenuState>().AsSingle();
        }
    }
}