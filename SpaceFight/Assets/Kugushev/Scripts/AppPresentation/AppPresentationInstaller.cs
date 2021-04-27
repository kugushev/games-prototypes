using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.AppPresentation.ViewModels;
using Kugushev.Scripts.Common.Modes;
using Zenject;

namespace Kugushev.Scripts.AppPresentation
{
    public class AppPresentationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MainMenuViewModel>().FromComponentInHierarchy().AsSingle();

            InstallSignalBus();
        }

        private void InstallSignalBus()
        {
            SignalBusInstaller.Install(Container);

            Container.SetupTransitiveSignal<GameModeParameters>();
        }
    }
}