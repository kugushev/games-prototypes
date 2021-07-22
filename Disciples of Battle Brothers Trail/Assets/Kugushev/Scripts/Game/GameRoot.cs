using Kugushev.Scripts.Common.Ecs;
using Kugushev.Scripts.Common.Managers;
using Kugushev.Scripts.Game.Factories.Abstractions;
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

namespace Kugushev.Scripts.Game
{
    public class GameRoot : BaseRoot
    {
        [SerializeField] private WorldView worldView;
        [SerializeField] private BaseEntitiesFactory[] entitiesFactories;
        [SerializeField] private GameModeManager gameModeManager;

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
            
            ecsSystems.Inject(gameModeManager);
        }

        private void InjectModelViews(EcsSystems ecsSystems)
        {
            var world = new World();
            worldView.Init(world);
            ecsSystems.Inject(world);
            ecsSystems.Inject(worldView);
        }

        private void InjectFactories(EcsSystems ecsSystems)
        {
            foreach (var factory in entitiesFactories) 
                ecsSystems.Inject(factory);
        }
    }
}