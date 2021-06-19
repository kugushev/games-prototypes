using System;
using System.Collections.Generic;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Interfaces;
using UniRx;
using UnityEngine;
using Kugushev.Scripts.Common.Core.Enums;

namespace Kugushev.Scripts.Game.Core.AI.Presets
{
    public abstract class ActiveAgent : IAgent
    {
        private readonly Queue<Vector2> _pathfindingBuffer = new Queue<Vector2>();
        private Vector2 _lastVisitedPoint;
        private DateTime? _interruptionTime;

        private readonly ReactiveProperty<Position> _position = new ReactiveProperty<Position>();

        private readonly ReactiveProperty<Direction2d> _direction =
            new ReactiveProperty<Direction2d>(Direction2d.Down);

        private readonly ReactiveProperty<ActivityType> _activity =
            new ReactiveProperty<ActivityType>(ActivityType.Stay);

        protected ActiveAgent(Position position)
        {
            _position.Value = position;
        }

        public IReadOnlyReactiveProperty<Position> Position => _position;
        public IReadOnlyReactiveProperty<Direction2d> Direction => _direction;
        public IReadOnlyReactiveProperty<ActivityType> Activity => _activity;

        public IOrder? CurrentOrder { get; set; }

        protected abstract bool IsActive { get; }
        protected abstract float InteractionRadius { get; }
        protected abstract float Speed { get; }
        protected abstract bool CanProcessOrder();
        protected abstract void CancelAttack();
        protected abstract bool CheckCollisions(Vector2 movement, float movementDistance, Vector2 target);

        void IAgent.ProcessCurrentOrder(DeltaTime delta)
        {
            if (IsActive)
                return;

            if (!CanProcessOrder())
                return;

            OrderProcessingStatus processingStatus;

            switch (CurrentOrder)
            {
                case OrderMove orderMove:
                    processingStatus = Process(orderMove, delta);
                    break;
                case OrderInteract orderInteract:
                    processingStatus = Process(orderInteract, delta);
                    break;
                case null:
                    processingStatus = OrderProcessingStatus.None;
                    break;
                default:
                    Debug.LogError($"Unexpected order {CurrentOrder}");
                    processingStatus = OrderProcessingStatus.None;
                    break;
            }

            if (processingStatus == OrderProcessingStatus.Finished)
                CurrentOrder = null;
        }

        private OrderProcessingStatus Process(OrderMove order, DeltaTime delta)
        {
            CancelAttack();

            var destinationReached = MoveToPosition(order.Target, delta);

            return destinationReached ? OrderProcessingStatus.Finished : OrderProcessingStatus.InProgress;
        }

        private OrderProcessingStatus Process(OrderInteract order, DeltaTime delta)
        {
            if (order.Interactable.IsInteractable)
                return OrderProcessingStatus.Finished;

            if (!DistanceCheck(order, delta, out var enemyPosition))
                return OrderProcessingStatus.InProgress;

            if (!DirectionCheck(enemyPosition))
                return OrderProcessingStatus.InProgress;

            ProcessInteraction(order);

            return OrderProcessingStatus.InProgress;
        }

        protected abstract void ProcessInteraction(OrderInteract order);

        private bool DistanceCheck(OrderInteract order, DeltaTime delta, out Position targetPosition)
        {
            targetPosition = order.Interactable.Position;
            if (Vector2.Distance(targetPosition.Vector, _position.Value.Vector) >= InteractionRadius)
            {
                CancelAttack();
                MoveToPosition(targetPosition, delta);
                return false;
            }

            return true;
        }

        private bool DirectionCheck(Position enemyPosition)
        {
            // we should always track direction to enemy, to rotate on moving target
            var direction = enemyPosition.Vector - _position.Value.Vector;
            var oldDirection = _direction.Value;
            _direction.Value = GetNewDirection(direction);
            if (_direction.Value != oldDirection)
            {
                CancelAttack();
                return false;
            }

            return true;
        }

        private bool MoveToPosition(Position target, DeltaTime delta)
        {
            var movement = GetNewPosition(target, delta);

            // update last visited point
            if (_position.Value.Vector != movement.Vector)
                _lastVisitedPoint = _position.Value.Vector;
            _position.Value = movement;

            var direction = _position.Value.Vector - _lastVisitedPoint;
            _direction.Value = GetNewDirection(direction);

            var distanceToTarget = Vector2.Distance(target.Vector, _position.Value.Vector);
            var destinationReached = distanceToTarget < GameConstants.Movement.Epsilon;
            _activity.Value = GetNewActivity(destinationReached);

            return destinationReached;
        }

        private Position GetNewPosition(Position target, DeltaTime delta)
        {
            var targetVector = target.Vector;
            var movementDistance = Speed * delta.Seconds;
            var movement = Vector2.MoveTowards(_position.Value.Vector, targetVector, movementDistance);

            _pathfindingBuffer.Clear();

            Vector2? finalMovement = null;

            for (int i = 0; i < 16; i++)
            {
                if (CheckCollisions(movement, movementDistance, targetVector))
                {
                    finalMovement = movement;
                    break;
                }

                movement = _pathfindingBuffer.Dequeue();
            }

            _pathfindingBuffer.Clear();


            if (finalMovement != null)
            {
                return new Position(finalMovement.Value);
            }

            Debug.LogWarning("Collision not resolved!");
            return _position.Value;
        }

        private Direction2d GetNewDirection(Vector2 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle < 0)
                angle = 360f + angle;

            if (angle > 45 && angle <= 135)
                return Direction2d.Up;
            if (angle > 135 && angle <= 225)
                return Direction2d.Left;
            if (angle > 225 && angle <= 315)
                return Direction2d.Down;
            return Direction2d.Right;
        }

        private ActivityType GetNewActivity(bool destinationReached) =>
            destinationReached ? ActivityType.Stay : ActivityType.Move;
    }
}