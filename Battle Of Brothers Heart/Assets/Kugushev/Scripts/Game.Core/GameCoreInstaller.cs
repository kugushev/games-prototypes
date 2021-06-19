using Kugushev.Scripts.Game.Core.Models.AI;
using Kugushev.Scripts.Game.Core.Models.AI.Orders;
using Kugushev.Scripts.Game.Core.ValueObjects;
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