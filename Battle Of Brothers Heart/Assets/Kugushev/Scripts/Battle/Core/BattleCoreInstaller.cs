using Kugushev.Scripts.Battle.Core.Models;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Battle.Core.Services;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Parameters;
using Zenject;

namespace Kugushev.Scripts.Battle.Core
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

            Container.Bind<SimpleAIService>().AsSingle();
            Container.Bind<Battlefield>().AsSingle();
        }
    }
}