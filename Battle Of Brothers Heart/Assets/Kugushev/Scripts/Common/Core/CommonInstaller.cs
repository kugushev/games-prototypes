using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.Controllers;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Common.Core
{
    public class CommonInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
            Container.BindInterfacesAndSelfTo<AgentsManager>().AsSingle();

            Container.BindFactory<Position, OrderMove, OrderMove.Factory>().FromPoolableMemoryPool();
        }
    }
}