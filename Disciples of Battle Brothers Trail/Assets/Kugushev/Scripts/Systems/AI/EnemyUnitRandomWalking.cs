using Kugushev.Scripts.Common;
using Kugushev.Scripts.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Systems.AI
{
    [CampaignSystem]
    public class EnemyUnitRandomWalking : IEcsRunSystem
    {
        private EcsFilter<UnitMove, AIIntention> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var move = ref _filter.Get1(i);
                move.Direction.x = Random.Range(-1, 2);
                move.Direction.y = Random.Range(-1, 2);
            }
        }
    }
}