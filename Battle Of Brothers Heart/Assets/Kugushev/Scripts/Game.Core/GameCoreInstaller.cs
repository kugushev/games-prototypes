using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Game.Core
{
    public class GameCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AgentsManager>().AsSingle();

            Container.BindFactory<Position, OrderMove, OrderMove.Factory>().FromPoolableMemoryPool();
        }
    }
}