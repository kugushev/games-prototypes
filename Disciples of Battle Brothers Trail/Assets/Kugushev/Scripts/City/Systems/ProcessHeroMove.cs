using System;
using Kugushev.Scripts.City.Components;
using Kugushev.Scripts.City.Components.Commands;
using Kugushev.Scripts.City.Models;
using Kugushev.Scripts.City.Models.Cells;
using Kugushev.Scripts.Common.Ecs.Components;
using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Components.Commands;
using Kugushev.Scripts.Game.Components.ViewRefs;
using Kugushev.Scripts.Game.Enums;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.City.Systems
{
    public class ProcessHeroMove : IEcsRunSystem
    {
        private EcsFilter<UnitMoveCommand, UnitGridPosition, HeroUnitViewRef>.Exclude<InteractCommand> _filter;

        private CityStructure _cityStructure;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var moveCommand = ref _filter.Get1(in i);
                ref var gridPosition = ref _filter.Get2(in i);

                if (gridPosition.Moving || moveCommand.Direction == Direction2d.None)
                    continue;

                var targetCoords = GetTargetCoords(ref gridPosition, ref moveCommand);

                var targetCell = _cityStructure.GetCell(targetCoords);

                switch (targetCell)
                {
                    case EmptyPavementCell _:
                        ApplyMovement(ref gridPosition, ref moveCommand, ref targetCoords);
                        break;
                    case InteractableCell interactableCell:
                        OrderInteractionCommand(i, interactableCell);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(targetCell));
                }

                moveCommand.Direction = Direction2d.None;
            }
        }

        private static Vector2Int GetTargetCoords(ref UnitGridPosition position, ref UnitMoveCommand moveCommand)
        {
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

            return actual;
        }

        private void ApplyMovement(ref UnitGridPosition gridPosition, ref UnitMoveCommand moveCommand,
            ref Vector2Int targetPosition)
        {
            gridPosition.PreviousPosition = gridPosition.ActualPosition;
            gridPosition.ActualPosition = targetPosition;
            gridPosition.Direction = moveCommand.Direction;
            gridPosition.Moving = true;
            gridPosition.Stopped = false;
        }

        private void OrderInteractionCommand(int i, InteractableCell interactableCell)
        {
            var entity = _filter.GetEntity(in i);
            entity
                .Replace(new InteractCommand(interactableCell.Interactable))
                .Replace(new InteractionsLock());
        }
    }
}