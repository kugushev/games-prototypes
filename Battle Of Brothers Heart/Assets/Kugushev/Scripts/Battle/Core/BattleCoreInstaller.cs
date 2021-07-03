using Kugushev.Scripts.Battle.Core.Models;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Core.Services;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using Zenject;

namespace Kugushev.Scripts.Battle.Core
{
    public class BattleCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BattleSupervisor>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSquad>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySquad>().AsSingle();

            Container.BindFactory<BaseFighter, OrderAttack, OrderAttack.Factory>().FromPoolableMemoryPool();

            Container.Bind<SimpleAIService>().AsSingle();
            Container.Bind<Battlefield>().AsSingle();
        }
    }
}