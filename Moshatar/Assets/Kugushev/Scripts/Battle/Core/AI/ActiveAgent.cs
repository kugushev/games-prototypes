using System.Collections.Generic;
using Kugushev.Scripts.Battle.Core.AI.Orders;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.AI
{
    public abstract class ActiveAgent : IAgent
    {
        private readonly Queue<Vector2> _pathfindingBuffer = new Queue<Vector2>();
        private Vector2 _lastVisitedPoint;

        protected readonly ReactiveProperty<Position> PositionImpl = new ReactiveProperty<Position>();

        protected readonly ReactiveProperty<ActivityType> ActivityImpl =
            new ReactiveProperty<ActivityType>(ActivityType.Stay);

        protected ActiveAgent(Position battlefieldPosition)
        {
            PositionImpl.Value = battlefieldPosition;
        }

        public IReadOnlyReactiveProperty<Position> Position => PositionImpl;
        public IReadOnlyReactiveProperty<ActivityType> Activity => ActivityImpl;

        public IOrder? CurrentOrder { get; set; }

        protected abstract bool IsActive { get; }
        protected abstract float InteractionRadius { get; }
        protected abstract float Speed { get; }
        protected abstract bool CanProcessOrder();
        protected abstract void CancelInteraction();
        protected abstract bool CheckCollisions(Vector2 movement, float movementDistance, Vector2 target);

        void IAgent.ProcessCurrentOrder(DeltaTime delta)
        {
            if (!IsActive)
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
            CancelInteraction();

            var destinationReached = MoveToPosition(order.Target, delta);

            return destinationReached ? OrderProcessingStatus.Finished : OrderProcessingStatus.InProgress;
        }

        private OrderProcessingStatus Process(OrderInteract order, DeltaTime delta)
        {
            if (!order.Interactable.IsInteractable)
                return OrderProcessingStatus.Finished;

            if (!DistanceCheck(order, delta, out var enemyPosition))
                return OrderProcessingStatus.InProgress;

            if (!DirectionCheck(enemyPosition))
                return OrderProcessingStatus.InProgress;

            return ProcessInteraction(order);
        }

        protected abstract OrderProcessingStatus ProcessInteraction(OrderInteract order);

        private bool DistanceCheck(OrderInteract order, DeltaTime delta, out Position targetPosition)
        {
            targetPosition = order.Interactable.Position;
            if (Vector2.Distance(targetPosition.Vector, PositionImpl.Value.Vector) >= InteractionRadius)
            {
                CancelInteraction();
                MoveToPosition(targetPosition, delta);
                return false;
            }

            return true;
        }

        private bool DirectionCheck(Position enemyPosition)
        {
            // todo: we should always track direction to enemy, to rotate on moving target
            // var direction = enemyPosition.Vector - PositionImpl.Value.Vector;
            // var oldDirection = DirectionImpl.Value;
            // DirectionImpl.Value = GetNewDirection(direction);
            // if (DirectionImpl.Value != oldDirection)
            // {
            //     CancelInteraction();
            //     return false;
            // }

            return true;
        }

        private bool MoveToPosition(Position target, DeltaTime delta)
        {
            var movement = GetNewPosition(target, delta);

            // update last visited point
            if (PositionImpl.Value.Vector != movement.Vector)
                _lastVisitedPoint = PositionImpl.Value.Vector;
            PositionImpl.Value = movement;

            var distanceToTarget = Vector2.Distance(target.Vector, PositionImpl.Value.Vector);
            var destinationReached = distanceToTarget < CommonConstants.Movement.Epsilon;
            ActivityImpl.Value = GetNewActivity(destinationReached);

            return destinationReached;
        }

        private Position GetNewPosition(Position target, DeltaTime delta)
        {
            var targetVector = target.Vector;
            var movementDistance = Speed * delta.Seconds;
            var movement = Vector2.MoveTowards(PositionImpl.Value.Vector, targetVector, movementDistance);

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
            return PositionImpl.Value;
        }

        private ActivityType GetNewActivity(bool destinationReached) =>
            destinationReached ? ActivityType.Stay : ActivityType.Move;

        protected void EnqueueToBuffer(Vector2 newPoint)
        {
            if (newPoint != _lastVisitedPoint)
                _pathfindingBuffer.Enqueue(newPoint);
        }
    }
}