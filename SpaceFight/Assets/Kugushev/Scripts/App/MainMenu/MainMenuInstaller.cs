using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.App.MainMenu.PresentationModels;
using Kugushev.Scripts.Common.ContextManagement;
using Zenject;

namespace Kugushev.Scripts.App.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MainMenuPresentationModel>().FromComponentInHierarchy().AsSingle();

            InstallSignals();
        }

        private void InstallSignals()
        {
            Container.InstallTransitiveSignal<GameParameters>();
        }
    }
}