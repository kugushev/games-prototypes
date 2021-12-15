using Kugushev.Scripts.Battle.Core.AI;
using Kugushev.Scripts.Battle.Core.AI.Orders;
using Kugushev.Scripts.Battle.Core.Models;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Core.Services;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Core
{
    public class BattleCoreInstaller : MonoInstaller
    {
        [SerializeField] private Director director;

        public override void InstallBindings()
        {
            Container.Bind<BattleGameplayManager>().FromInstance(BattleGameplayManager.Instance).AsSingle();
            
            Container.BindInterfacesAndSelfTo<BattleSupervisor>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSquad>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySquad>().AsSingle();

            Container.BindFactory<BaseFighter, OrderAttack, OrderAttack.Factory>().FromPoolableMemoryPool();

            Container.Bind<SimpleAIService>().AsSingle();
            Container.Bind<Battlefield>().AsSingle();


            Container.BindFactory<Position, OrderMove, OrderMove.Factory>().FromPoolableMemoryPool();
            Container.BindInterfacesAndSelfTo<AgentsManager>().AsSingle();

            if (director is { })
                Container.Bind<Director>().FromInstance(director).AsSingle();
        }
    }
}