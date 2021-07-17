using Kugushev.Scripts.Common;
using Kugushev.Scripts.Components;
using Kugushev.Scripts.Components.Commands;
using Kugushev.Scripts.Components.ViewRefs;
using Leopotam.Ecs;

namespace Kugushev.Scripts.Systems.UpdateView
{
    [CampaignSystem]
    public class UnitUpdateTransformView : IEcsRunSystem
    {
        private EcsFilter<UnitTransformViewRef, UnitTransform, UnitGridPosition> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var viewRef = ref _filter.Get1(i);
                ref var transform = ref _filter.Get2(i);
                ref var position = ref _filter.Get3(i);

                viewRef.View.UpdatePosition(transform.Position);
                viewRef.View.UpdateDirection(position.Direction);
                viewRef.View.UpdateIsMoving(position.Moving);
            }
        }
    }
}