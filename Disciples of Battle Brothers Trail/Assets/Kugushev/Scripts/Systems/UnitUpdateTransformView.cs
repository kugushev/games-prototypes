using Kugushev.Scripts.Common;
using Kugushev.Scripts.Components;
using Kugushev.Scripts.Components.ViewRefs;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Systems
{
    [CampaignSystem]
    public class UnitUpdateTransformView : IEcsRunSystem
    {
        private EcsFilter<UnitTransformViewRef, UnitFloatPosition> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var viewRef = ref _filter.Get1(i);
                ref var position = ref _filter.Get2(i);

                viewRef.View.transform.position = new Vector3(position.X, position.Y);
            }
        }
    }
}