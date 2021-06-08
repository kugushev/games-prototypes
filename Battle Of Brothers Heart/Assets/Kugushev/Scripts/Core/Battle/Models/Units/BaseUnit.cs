using System;
using Kugushev.Scripts.Core.Battle.Enums;
using Kugushev.Scripts.Core.Battle.Interfaces;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Core.Battle.Models.Units
{
    public class BaseUnit
    {
        // todo: create class Status and expose it as reactive property. It should have inheritors: Idle, Moving, Attacking

        protected readonly ReactiveProperty<Position> _position = new ReactiveProperty<Position>();

        private readonly ReactiveProperty<UnitDirection> _direction =
            new ReactiveProperty<UnitDirection>(UnitDirection.Down);

        private readonly ReactiveProperty<UnitActivity> _activity =
            new ReactiveProperty<UnitActivity>(UnitActivity.Stay);

        private DateTime _lastAttackTime = DateTime.MinValue;

        public IReadOnlyReactiveProperty<Position> Position => _position;
        public IReadOnlyReactiveProperty<UnitDirection> Direction => _direction;
        public IReadOnlyReactiveProperty<UnitActivity> Activity => _activity;

        public IOrder? CurrentOrder { get; set; }
        
        public event Action? Attacking;
        public event Action? Hurt;
        
        public void Suffer()
        {
            Hurt?.Invoke();
        }
        
        internal void ProcessCurrentOrder(DeltaTime delta)
        {
            OrderProcessingStatus processingStatus;

            switch (CurrentOrder)
            {
                case OrderMove orderMove:
                    processingStatus = Process(orderMove, delta);
                    break;
                case OrderAttack orderAttack:
                    processingStatus = Process(orderAttack, delta);
                    break;
                case null:
                    processingStatus = OrderProcessingStatus.None;
                    break;
                default:
                    Debug.LogError($"Unexpected order {CurrentOrder}");
                    processingStatus = OrderProcessingStatus.None;
                    break;
            }

            if (processingStatus == OrderProcessingStatus.Completed)
                CurrentOrder = null;
        }

        private OrderProcessingStatus Process(OrderMove order, DeltaTime delta)
        {
            var destinationReached = MoveToPosition(order.Target, delta);

            return destinationReached ? OrderProcessingStatus.Completed : OrderProcessingStatus.InProgress;
        }

        private OrderProcessingStatus Process(OrderAttack order, DeltaTime delta)
        {
            var enemyPosition = order.Target.Position.Value;
            if (Vector2.Distance(enemyPosition.Vector, _position.Value.Vector) >= BattleConstants.SwordAttackRange)
            {
                MoveToPosition(enemyPosition, delta);
            }
            else
            {
                _activity.Value = UnitActivity.Stay;
                
                var direction = enemyPosition.Vector - _position.Value.Vector;
                _direction.Value = GetNewDirection(direction);
                
                if (DateTime.Now - _lastAttackTime > BattleConstants.SwordAttackCooldown)
                {
                    order.Target.Suffer();

                    Attacking?.Invoke();
                    _lastAttackTime = DateTime.Now;
                }
            }

            return OrderProcessingStatus.InProgress;
        }

        private bool MoveToPosition(Position target, DeltaTime delta)
        {
            _position.Value = GetNewPosition(target, delta);

            var direction = target.Vector - _position.Value.Vector;
            _direction.Value = GetNewDirection(direction);

            var destinationReached = direction.sqrMagnitude < BattleConstants.UnitToTargetEpsilon;
            _activity.Value = GetNewActivity(destinationReached);

            return destinationReached;
        }

        private Position GetNewPosition(Position target, DeltaTime delta)
        {
            var targetVector = target.Vector;
            var newVector = Vector2.MoveTowards(_position.Value.Vector, targetVector,
                BattleConstants.UnitSpeed * delta.Seconds);

            return new Position(newVector);
        }

        private UnitDirection GetNewDirection(Vector2 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle < 0)
                angle = 360f + angle;

            if (angle > 45 && angle <= 135)
                return UnitDirection.Up;
            if (angle > 135 && angle <= 225)
                return UnitDirection.Left;
            if (angle > 225 && angle <= 315)
                return UnitDirection.Down;
            return UnitDirection.Right;
        }

        private UnitActivity GetNewActivity(bool destinationReached) =>
            destinationReached ? UnitActivity.Stay : UnitActivity.Move;
    }
}