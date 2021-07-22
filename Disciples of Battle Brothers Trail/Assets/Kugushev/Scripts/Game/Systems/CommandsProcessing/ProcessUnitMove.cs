using System;
using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Components.Commands;
using Kugushev.Scripts.Game.Components.ViewRefs;
using Kugushev.Scripts.Game.Enums;
using Leopotam.Ecs;

namespace Kugushev.Scripts.Game.Systems.CommandsProcessing
{
    public class ProcessUnitMove : IEcsRunSystem
    {
        private EcsFilter<UnitMove, UnitGridPosition, HeroUnitViewRef> _filterHero;
        private EcsFilter<UnitMove, UnitGridPosition>.Exclude<HeroUnitViewRef> _filterOther;

        public void Run()
        {
            bool allowMoving = false;
            foreach (var i in _filterHero)
            {
                ref var move = ref _filterHero.Get1(i);
                ref var position = ref _filterHero.Get2(i);

                allowMoving |= HandleMoving(ref position, ref move);
            }

            if (!allowMoving)
                return;

            foreach (var i in _filterOther)
            {
                ref var move = ref _filterOther.Get1(i);
                ref var position = ref _filterOther.Get2(i);

                HandleMoving(ref position, ref move);
            }
        }

        private static bool HandleMoving(ref UnitGridPosition position, ref UnitMove move)
        {
            if (position.Moving || move.Direction == Direction2d.None)
                return false;

            var actual = position.ActualPosition;

            switch (move.Direction)
            {
                case Direction2d.Up:
                    actual.y += 1;
                    break;
                case Direction2d.Down:
                    actual.y -= 1;
                    break;
                case Direction2d.Right:
                    actual.x += 1;
                    break;
                case Direction2d.Left:
                    actual.x -= 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(move.Direction), move.Direction,
                        "Unexpected direction");
            }

            position.PreviousPosition = position.ActualPosition;
            position.ActualPosition = actual;
            position.Direction = move.Direction;
            position.Moving = true;
            position.Stopped = false;

            return true;
        }
    }
}