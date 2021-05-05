using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.AppPresentation.PresentationModels;
using Kugushev.Scripts.Common.ContextManagement;
using Zenject;

namespace Kugushev.Scripts.AppPresentation
{
    public class AppPresentationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MainMenuPresentationModel>().FromComponentInHierarchy().AsSingle();

            InstallSignals();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.InstallTransitiveSignal<GameParameters>();
        }
    }
}