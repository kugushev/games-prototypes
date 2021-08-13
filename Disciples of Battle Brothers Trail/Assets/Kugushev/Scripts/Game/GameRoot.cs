using Kugushev.Scripts.Common.Ecs;
using Kugushev.Scripts.Game.Factories.Abstractions;
using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Systems;
using Kugushev.Scripts.Game.Systems.AI;
using Kugushev.Scripts.Game.Systems.CommandsProcessing;
using Kugushev.Scripts.Game.Systems.Input;
using Kugushev.Scripts.Game.Systems.Interactions;
using Kugushev.Scripts.Game.Systems.UpdateView;
using Kugushev.Scripts.Game.Views;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game
{
    public class GameRoot : BaseRoot
    {
        [SerializeField] private WorldView worldView;
        [SerializeField] private BaseEntitiesFactory[] entitiesFactories;

        [Inject] private DiContainer _container;

        protected override void InitSystems(EcsSystems ecsSystems)
        {
            ecsSystems
                .Add(new EnemyUnitRandomWalking())
                .Add(new ProcessUnitMove())
                .Add(new PlayerInputDetection())
                .Add(new HeroInteractions())
                .Add(new UnitUpdateTransformView())
                .Add(new UnitAnimateMoving(GameConstants.Units.Speed))
                .Add(new UnitsSpawner());
        }

        protected override void Inject(EcsSystems ecsSystems)
        {
            InjectModelViews(ecsSystems);
            InjectFactories(ecsSystems);

            ecsSystems.Inject(_container.Resolve<GameModeManager>());
        }

        private void InjectModelViews(EcsSystems ecsSystems)
        {
            var world = new World();
            worldView.Init(world);
            ecsSystems.Inject(world);
            ecsSystems.Inject(worldView);

            ecsSystems.Inject(new CellsVisitingManager());
        }

        private void InjectFactories(EcsSystems ecsSystems)
        {
            foreach (var factory in entitiesFactories)
                ecsSystems.Inject(factory);
        }
    }
}