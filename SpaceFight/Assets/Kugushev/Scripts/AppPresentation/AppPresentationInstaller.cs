using Kugushev.Scripts.App;
using Kugushev.Scripts.AppPresentation.Signals;
using Kugushev.Scripts.AppPresentation.ViewModels;
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

            Container.DeclareSignal<NewGameSignal>();
            Container.BindMemoryPool<NewGameSignal, NewGameSignal.Pool>();
            Container.BindSignal<NewGameSignal>()
                .ToMethod<AppSceneLoader>((loader, signal) =>
                {
                    loader.LoadNewGame(signal.Parameters);
                    signal.DespawnSelf();
                })
                .FromResolveAll();
        }
    }
}