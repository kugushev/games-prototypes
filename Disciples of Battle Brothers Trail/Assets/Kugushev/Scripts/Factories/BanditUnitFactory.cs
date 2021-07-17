using Kugushev.Scripts.Components;
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

        public override EcsEntity Create(EcsWorld world)
        {
            var instance = Instantiate(prefab);
            var transformView = instance.GetComponent<UnitTransformView>();

            return world.NewEntity()
                .Replace(new UnitTransformViewRef(transformView))
                .Replace(new UnitMove())
                .Replace(new UnitFloatPosition())
                .Replace(new AIIntention());
        }
    }
}