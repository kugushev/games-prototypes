using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Components.Commands;
using Kugushev.Scripts.Game.Enums;
using Leopotam.Ecs;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.Systems.AI
{
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