using Kugushev.Scripts.Core.Battle.Models;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using Zenject;

namespace Kugushev.Scripts.Core.Battle
{
    public class BattleCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Squad>().AsSingle();
            
            Container.BindFactory<Position, OrderMove, OrderMove.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<EnemyUnit, OrderAttack, OrderAttack.Factory>().FromPoolableMemoryPool();
        }
    }
}