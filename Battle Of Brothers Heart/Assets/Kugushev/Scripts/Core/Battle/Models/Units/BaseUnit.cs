using System;
using Kugushev.Scripts.Core.Battle.Enums;
using Kugushev.Scripts.Core.Battle.Interfaces;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Core.Battle.Models.Units
{
    public abstract class BaseUnit
    {
        private DateTime? _interruptionTime;
        private AttackProcessing? _currentAttack;

        protected BaseUnit(Position position)
        {
            _position.Value = position;
        }

        protected readonly ReactiveProperty<Position> _position = new ReactiveProperty<Position>();

        private readonly ReactiveProperty<UnitDirection> _direction =
            new ReactiveProperty<UnitDirection>(UnitDirection.Down);

        private readonly ReactiveProperty<UnitActivity> _activity =
            new ReactiveProperty<UnitActivity>(UnitActivity.Staying);

        public IReadOnlyReactiveProperty<Position> Position => _position;
        public IReadOnlyReactiveProperty<UnitDirection> Direction => _direction;
        public IReadOnlyReactiveProperty<UnitActivity> Activity => _activity;

        public IOrder? CurrentOrder { get; set; }

        public event Action? Attacking;
        public event Action? AttackCanceled;
        public event Action? Hurt;

        public void Suffer()
        {
            _currentAttack = null;
            _activity.Value = UnitActivity.Staying;
            _interruptionTime = DateTime.Now;
            Hurt?.Invoke();
        }

        internal void ProcessCurrentOrder(DeltaTime delta)
        {
            if (_interruptionTime != null)
            {
                if (DateTime.Now < _interruptionTime + BattleConstants.HurtInterruptionTime)
                    return;

                _interruptionTime = null;
            }


            OrderProcessingStatus processingStatus;

            switch (CurrentOrder)
            {
                case OrderMove orderMove:
                    processingStatus = Process(orderMove, delta);
                    break;
                case OrderAttack orderAttack:
                    Process(orderAttack, delta);
                    // attacks are always in progress
                    processingStatus = OrderProcessingStatus.InProgress;
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
            CancelAttack();

            var destinationReached = MoveToPosition(order.Target, delta);

            return destinationReached ? OrderProcessingStatus.Completed : OrderProcessingStatus.InProgress;
        }

        private void Process(OrderAttack order, DeltaTime delta)
        {
            if (!DistanceCheck(order, delta, out var enemyPosition))
                return;

            if (!DirectionCheck(enemyPosition))
                return;

            switch (_currentAttack?.Status)
            {
                case null:
                    _activity.Value = UnitActivity.Staying;
                    _currentAttack = new AttackProcessing(AttackStatus.Preparing);
                    break;
                case AttackStatus.Preparing:
                    Attacking?.Invoke();
                    _currentAttack = new AttackProcessing(AttackStatus.Prepared);
                    break;
                case AttackStatus.Prepared:
                    if (_currentAttack.Value.IsReadyForNextStep())
                        _currentAttack = new AttackProcessing(AttackStatus.Executing);
                    break;
                case AttackStatus.Executing:
                    order.Target.Suffer();
                    _currentAttack = new AttackProcessing(AttackStatus.Executed);
                    break;
                case AttackStatus.Executed:
                    if (_currentAttack.Value.IsReadyForNextStep())
                        _currentAttack = new AttackProcessing(AttackStatus.OnCooldown);
                    break;
                case AttackStatus.OnCooldown:
                    if (_currentAttack.Value.IsReadyForNextStep())
                        _currentAttack = new AttackProcessing(AttackStatus.Preparing);
                    break;
                default:
                    Debug.LogError($"Unexpected status {_currentAttack?.Status}");
                    break;
            }
        }

        private bool DistanceCheck(OrderAttack order, DeltaTime delta, out Position enemyPosition)
        {
            enemyPosition = order.Target.Position.Value;
            if (Vector2.Distance(enemyPosition.Vector, _position.Value.Vector) >= BattleConstants.SwordAttackRange)
            {
                CancelAttack();
                MoveToPosition(enemyPosition, delta);
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
            destinationReached ? UnitActivity.Staying : UnitActivity.Moving;

        private void CancelAttack()
        {
            if (_currentAttack != null)
            {
                _currentAttack = null;
                AttackCanceled?.Invoke();
            }
        }
    }
}