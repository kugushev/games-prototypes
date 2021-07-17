using Kugushev.Scripts.Common;
using Kugushev.Scripts.Components;
using Kugushev.Scripts.Views;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Systems
{
    [CampaignSystem]
    public class UnitAnimateMoving : IEcsRunSystem
    {
        private EcsFilter<UnitGridPosition, UnitTransform> _filter;

        private WorldView _worldView;


        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var position = ref _filter.Get1(i);
                ref var transform = ref _filter.Get2(i);

                if (!position.Moving)
                    continue;

                var actual = _worldView.CellToWorld(position.ActualPosition);
                var previous = _worldView.CellToWorld(position.PreviousPosition);

                var directionVector = (actual - previous).normalized;
                transform.Position += directionVector * Time.deltaTime * CampaignConstants.Units.Speed;

                if (Vector3.Dot(directionVector, actual - transform.Position) < 0)
                    position.Moving = false;
            }
        }
    }
}