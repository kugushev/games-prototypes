using Kugushev.Scripts.Components;
using Kugushev.Scripts.Components.Commands;
using Kugushev.Scripts.Components.ViewRefs;
using Kugushev.Scripts.Factories.Abstractions;
using Kugushev.Scripts.Views;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Factories
{
    [CreateAssetMenu]
    public class PlayerUnitFactory : BaseEntitiesFactory
    {
        [SerializeField] private GameObject prefab;

        public EcsEntity Create(EcsWorld world, WorldView worldView)
        {
            var instance = Instantiate(prefab, worldView.UnitsRoot);
            var transformView = instance.GetComponent<UnitTransformView>();
            var playerView = instance.GetComponent<HeroUnitView>();

            var start = new Vector2Int(0, 0);
            var startWorld = worldView.CellToWorld(start);

            return world.NewEntity()
                .Replace(new UnitTransformViewRef(transformView))
                .Replace(new HeroUnitViewRef(playerView))
                .Replace(new UnitMove())
                .Replace(new UnitGridPosition {ActualPosition = start}) // todo: take from save file
                .Replace(new UnitTransform {Position = startWorld});
        }
    }
}