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
    public class BanditUnitFactory : BaseEntitiesFactory
    {
        [SerializeField] private GameObject prefab;

        public EcsEntity Create(EcsWorld world, WorldView worldView)
        {
            var instance = Instantiate(prefab, worldView.UnitsRoot);
            var transformView = instance.GetComponent<UnitTransformView>();

            var start = new Vector2Int {x = Random.Range(-10, 10), y = Random.Range(-5, 5)};
            var startWorld = worldView.CellToWorld(start);

            return world.NewEntity()
                .Replace(new UnitTransformViewRef(transformView))
                .Replace(new UnitMove())
                .Replace(new UnitGridPosition {ActualPosition = start})
                .Replace(new UnitTransform {Position = startWorld})
                .Replace(new AIIntention());
        }
    }
}