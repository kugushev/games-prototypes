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
        private EcsFilter<UnitMoveCommand, UnitGridPosition, HeroUnitViewRef> _filterHero;
        private EcsFilter<UnitMoveCommand, UnitGridPosition>.Exclude<HeroUnitViewRef> _filterOther;

        public void Run()
        {
            bool allowMoving = false;
            foreach (var i in _filterHero)
            {
                ref var move = ref _filterHero.Get1(i);
                ref var position = ref _filterHero.Get2(i);
                var entity = _filterHero.GetEntity(i);

                allowMoving |= HandleMoving(ref position, ref move, entity);
            }

            if (!allowMoving)
                return;

            foreach (var i in _filterOther)
            {
                ref var move = ref _filterOther.Get1(i);
                ref var position = ref _filterOther.Get2(i);
                var entity = _filterOther.GetEntity(i);

                HandleMoving(ref position, ref move, entity);
            }
        }

        private static bool HandleMoving(ref UnitGridPosition position, ref UnitMoveCommand moveCommand,
            EcsEntity ecsEntity)
        {
            if (position.Moving || moveCommand.Direction == Direction2d.None)
                return false;

            var actual = position.ActualPosition;

            switch (moveCommand.Direction)
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
                    throw new ArgumentOutOfRangeException(nameof(moveCommand.Direction), moveCommand.Direction,
                        "Unexpected direction");
            }

            position.PreviousPosition = position.ActualPosition;
            position.ActualPosition = actual;
            position.Direction = moveCommand.Direction;
            position.Moving = true;
            position.Stopped = false;

            ecsEntity.Replace(new UnitMoveOneStepEvent(actual, position.PreviousPosition));

            return true;
        }
    }
}