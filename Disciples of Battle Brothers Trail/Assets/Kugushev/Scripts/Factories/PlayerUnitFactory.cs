using Kugushev.Scripts.Components;
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

        public override EcsEntity Create(EcsWorld world)
        {
            var instance = Instantiate(prefab);
            var transformView = instance.GetComponent<UnitTransformView>();
            var playerView = instance.GetComponent<HeroUnitView>();

            return world.NewEntity()
                .Replace(new UnitTransformViewRef(transformView))
                .Replace(new HeroUnitViewRef(playerView))
                .Replace(new UnitMove())
                .Replace(new UnitFloatPosition());
        }
    }
}