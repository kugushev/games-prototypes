using Kugushev.Scripts.App.Core.ContextManagement;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Core.Signals;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Tests.Integration.Game.Setup
{
    public class PoliticsTestingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<GameState>().AsSingle();

            Container.InstallPoolable<Intrigue, IntrigueCard, IntrigueCard.Factory>();
            Container.InstallPoolable<Intrigue, IntrigueCardObtained, IntrigueCardObtained.Factory>();

            Container.DeclareSignal<IntrigueCardObtained>();
        }
    }
}