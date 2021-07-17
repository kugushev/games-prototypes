using System;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Components;
using Kugushev.Scripts.Components.Commands;
using Kugushev.Scripts.Enums;
using Leopotam.Ecs;
using Random = UnityEngine.Random;

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
                move.Direction = (Direction2d) Random.Range(1, 5);
            }
        }
    }
}