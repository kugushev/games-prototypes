using Kugushev.Scripts.Core.Battle.Models;
using Kugushev.Scripts.Core.Battle.Models.Squad;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using Kugushev.Scripts.Core.Game.Models;
using Kugushev.Scripts.Core.Game.Parameters;
using Zenject;

namespace Kugushev.Scripts.Core.Battle
{
    public class BattleCoreInstaller : MonoInstaller
    {
        // todo: make it subcontainer with parameters: https://github.com/modesttree/Zenject/blob/master/Documentation/SubContainers.md

        [InjectOptional] private BattleParameters? _battleParameters;

        public override void InstallBindings()
        {
            Container.Bind<BattleParameters>().FromInstance(_battleParameters ?? new BattleParameters(
                // new[] {new Teammate()},
                // new[] {new Enemy()}));
                new[] {new Teammate(), new Teammate(), new Teammate(), new Teammate()},
            new[] {new Enemy(), new Enemy(), new Enemy(), new Enemy()}));

            Container.BindInterfacesAndSelfTo<PlayerSquad>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySquad>().AsSingle();

            Container.BindFactory<Position, OrderMove, OrderMove.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<BaseUnit, OrderAttack, OrderAttack.Factory>().FromPoolableMemoryPool();
        }
    }
}