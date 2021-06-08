using Kugushev.Scripts.Core.Battle.Models;
using Kugushev.Scripts.Core.Battle.Models.Squad;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using Zenject;

namespace Kugushev.Scripts.Core.Battle
{
    public class BattleCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerSquad>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySquad>().AsSingle();

            Container.BindFactory<Position, OrderMove, OrderMove.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<BaseUnit, OrderAttack, OrderAttack.Factory>().FromPoolableMemoryPool();
        }
    }
}