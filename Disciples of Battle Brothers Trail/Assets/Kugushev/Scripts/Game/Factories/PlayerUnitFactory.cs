using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Components.Commands;
using Kugushev.Scripts.Game.Components.Refs;
using Kugushev.Scripts.Game.Components.ViewRefs;
using Kugushev.Scripts.Game.Factories.Abstractions;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Kugushev.Scripts.Game.Views;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game.Factories
{
    [CreateAssetMenu]
    public class PlayerUnitFactory : BaseEntitiesFactory
    {
        [SerializeField] private GameObject prefab;

        public EcsEntity Create(EcsWorld world, WorldView worldView, Hero hero)
        {
            var instance = Instantiate(prefab, worldView.UnitsRoot);
            var transformView = instance.GetComponent<UnitTransformView>();
            var playerView = instance.GetComponent<HeroUnitView>();

            var start = new Vector2Int(GameConstants.World.Width / 2, GameConstants.World.Height / 2);
            var startWorld = worldView.CellToWorld(start);

            return world.NewEntity()
                .Replace(new UnitTransformViewRef(transformView))
                .Replace(new ModelRef<IInteractable>(hero))
                .Replace(new HeroUnitViewRef(playerView))
                .Replace(new UnitMoveCommand())
                .Replace(new UnitGridPosition {ActualPosition = start}) // todo: take from save file
                .Replace(new UnitTransform {Position = startWorld});
        }
    }
}