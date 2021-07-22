using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Views;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Game.Systems
{
    public class UnitAnimateMoving : IEcsRunSystem
    {
        private readonly float _speed;

        public UnitAnimateMoving(float speed)
        {
            _speed = speed;
        }
        
        private EcsFilter<UnitGridPosition, UnitTransform> _filter;

        private IGrid _grid;
        
        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var position = ref _filter.Get1(i);
                ref var transform = ref _filter.Get2(i);

                if (!position.Moving)
                {
                    position.Stopped = true;
                    continue;
                }

                var actual = _grid.CellToWorld(position.ActualPosition);
                var previous = _grid.CellToWorld(position.PreviousPosition);

                var directionVector = (actual - previous).normalized;
                transform.Position += directionVector * Time.deltaTime * _speed;

                if (Vector3.Dot(directionVector, actual - transform.Position) < 0)
                    position.Moving = false;
            }
        }
    }
}