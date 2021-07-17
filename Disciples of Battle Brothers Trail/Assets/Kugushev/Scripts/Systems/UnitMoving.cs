using Kugushev.Scripts.Common;
using Kugushev.Scripts.Components;
using Leopotam.Ecs;

namespace Kugushev.Scripts.Systems
{
    [CampaignSystem]
    public class UnitMoving : IEcsRunSystem
    {
        private EcsFilter<UnitMove, UnitFloatPosition> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var move = ref _filter.Get1(i);
                ref var position = ref _filter.Get2(i);

                position.X += move.Direction.x;
                position.Y += move.Direction.y;
            }
        }
    }
}