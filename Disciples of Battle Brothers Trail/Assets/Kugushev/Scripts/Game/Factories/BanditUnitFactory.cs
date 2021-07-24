using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Components.Commands;
using Kugushev.Scripts.Game.Components.ViewRefs;
using Kugushev.Scripts.Game.Factories.Abstractions;
using Kugushev.Scripts.Game.Views;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Game.Factories
{
    [CreateAssetMenu]
    public class BanditUnitFactory : BaseEntitiesFactory
    {
        [SerializeField] private GameObject prefab;

        public EcsEntity Create(EcsWorld world, WorldView worldView)
        {
            var instance = Instantiate(prefab, worldView.UnitsRoot);
            var transformView = instance.GetComponent<UnitTransformView>();

            var start = new Vector2Int
            {
                x = Random.Range(0, GameConstants.World.Width), 
                y = Random.Range(0, GameConstants.World.Height)
            };
            var startWorld = worldView.CellToWorld(start);

            return world.NewEntity()
                .Replace(new UnitTransformViewRef(transformView))
                .Replace(new UnitMoveCommand())
                .Replace(new UnitGridPosition {ActualPosition = start})
                .Replace(new UnitTransform {Position = startWorld})
                .Replace(new AIIntention());
        }
    }
}