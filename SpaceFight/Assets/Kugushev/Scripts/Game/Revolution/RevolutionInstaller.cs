using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Zenject;

namespace Kugushev.Scripts.Game.Revolution
{
    public class RevolutionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallSignals();
        }

        private void InstallSignals()
        {
            Container.InstallTransitiveSignal<GameExitParameters>();
        }
    }
}