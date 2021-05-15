using Kugushev.Scripts.App.Core.ContextManagement;
using Zenject;

namespace Kugushev.Scripts.Tests.Integration.Game.Setup
{
    public class PoliticsTestingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameState>().AsSingle();
        }
    }
}