using System;
using System.Collections.Generic;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Helpers;
using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Interfaces;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Models.Units
{
    public abstract class BaseUnit : IInteractable
    {
        private readonly Battlefield _battlefield;
        private readonly Queue<Vector2> _pathfindingBuffer = new Queue<Vector2>();
        private Vector2 _lastVisitedPoint;
        private DateTime? _interruptionTime;
        private AttackProcessing? _currentAttack;

        private readonly ReactiveProperty<int> _hitPoints = new ReactiveProperty<int>();
        private readonly ReactiveProperty<Position> _position = new ReactiveProperty<Position>();

        private readonly ReactiveProperty<Direction2d> _direction =
            new ReactiveProperty<Direction2d>(Common.Core.Enums.Direction2d.Down);

        private readonly ReactiveProperty<ActivityType> _activity =
            new ReactiveProperty<ActivityType>(Enums.ActivityType.Stay);

        protected BaseUnit(Position position, Battlefield battlefield)
        {
            _battlefield = battlefield;
            _position.Value = position;
            _hitPoints.Value = BattleConstants.UnitMaxHitPoints;
        }

        public IReadOnlyReactiveProperty<int> HitPoints => _hitPoints;
        public IReadOnlyReactiveProperty<Position> Position => _position;
        public IReadOnlyReactiveProperty<Direction2d> Direction => _direction;
        public IReadOnlyReactiveProperty<ActivityType> Activity => _activity;

        // todo: use aggregated class Person 
        public float WeaponRange => BattleConstants.SwordAttackRange;
        public int Damage => BattleConstants.SwordAttackDamage;

        public IOrder? CurrentOrder { get; set; }
        public bool IsDead => _activity.Value == Enums.ActivityType.Death;

        public event Action? Attacking;
        public event Action? AttackCanceled;
        public event Action<BaseUnit>? Hurt;
        public event Action? Die;

        #region IInteractable

        Position IInteractable.Position => _position.Value;
        bool IInteractable.IsInteractable => IsDead;

        #endregion

        public void Suffer(int damage, BaseUnit attacker)
        {
            _hitPoints.Value -= damage;

            if (_hitPoints.Value <= 0)
            {
                _activity.Value = Enums.ActivityType.Death;
                _currentAttack = null;
                CurrentOrder = null;
                Die?.Invoke();
                return;
            }

            _currentAttack = null;
            _activity.Value = Enums.ActivityType.Stay;
            _interruptionTime = DateTime.Now;
            Hurt?.Invoke(attacker);
        }

        internal void ProcessCurrentOrder(DeltaTime delta)
        {
            if (IsDead)
                return;

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

            if (processingStatus == OrderProcessingStatus.Finished)
                CurrentOrder = null;
        }

        private OrderProcessingStatus Process(OrderMove order, DeltaTime delta)
        {
            CancelAttack();

            var destinationReached = MoveToPosition(order.Target, delta);

            return destinationReached ? OrderProcessingStatus.Finished : OrderProcessingStatus.InProgress;
        }

        private OrderProcessingStatus Process(OrderAttack order, DeltaTime delta)
        {
            if (order.Target.IsDead)
                return OrderProcessingStatus.Finished;

            if (!DistanceCheck(order, delta, out var enemyPosition))
                return OrderProcessingStatus.InProgress;

            if (!DirectionCheck(enemyPosition))
                return OrderProcessingStatus.InProgress;

            switch (_currentAttack?.Status)
            {
                case null:
                    _activity.Value = Enums.ActivityType.Stay;
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
                    order.Target.Suffer(Damage, this);
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

            return OrderProcessingStatus.InProgress;
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
            var movement = GetNewPosition(target, delta);

            // update last visited point
            if (_position.Value.Vector != movement.Vector)
                _lastVisitedPoint = _position.Value.Vector;
            _position.Value = movement;

            var direction = _position.Value.Vector - _lastVisitedPoint;
            _direction.Value = GetNewDirection(direction);

            var distanceToTarget = Vector2.Distance(target.Vector, _position.Value.Vector);
            var destinationReached = distanceToTarget < BattleConstants.UnitToTargetEpsilon;
            _activity.Value = GetNewActivity(destinationReached);

            return destinationReached;
        }

        private Position GetNewPosition(Position target, DeltaTime delta)
        {
            var targetVector = target.Vector;
            var movementDistance = BattleConstants.UnitSpeed * delta.Seconds;
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

        private bool CheckCollisions(Vector2 movement, float movementDistance, Vector2 target)
        {
            foreach (var unit in _battlefield.Units)
            {
                if (unit != this)
                {
                    var unitVector = unit.Position.Value.Vector;
                    var distance = Vector2.Distance(unitVector, movement);
                    if (distance < BattleConstants.UnitRadius)
                    {
                        // use special radius
                        var unitRadius = BattleConstants.UnitRadius + movementDistance * 0.1f;
                        var solutions = MathHelper.FindCircleCircleIntersections(
                            _position.Value.Vector, movementDistance,
                            unitVector, unitRadius,
                            out var newPoint1, out var newPoint2);

                        switch (solutions)
                        {
                            case 0:
                                Debug.LogError("Wow, no intersections");
                                continue;
                            case 1:
                                _pathfindingBuffer.Enqueue(newPoint1);
                                Debug.LogWarning("Dot");
                                return false;
                            case 2:

                                var newPoint1Distance = Vector2.Distance(newPoint1, target);
                                var newPoint2Distance = Vector2.Distance(newPoint2, target);

                                if (newPoint1Distance > newPoint2Distance)
                                {
                                    EnqueueToBuffer(newPoint1);
                                    EnqueueToBuffer(newPoint2);
                                }
                                else
                                {
                                    EnqueueToBuffer(newPoint1);
                                    EnqueueToBuffer(newPoint2);
                                }

                                return false;
                        }
                    }
                }
            }

            return true;

            void EnqueueToBuffer(Vector2 newPoint)
            {
                if (newPoint != _lastVisitedPoint)
                    _pathfindingBuffer.Enqueue(newPoint);
            }
        }

        private Direction2d GetNewDirection(Vector2 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle < 0)
                angle = 360f + angle;

            if (angle > 45 && angle <= 135)
                return Common.Core.Enums.Direction2d.Up;
            if (angle > 135 && angle <= 225)
                return Common.Core.Enums.Direction2d.Left;
            if (angle > 225 && angle <= 315)
                return Common.Core.Enums.Direction2d.Down;
            return Common.Core.Enums.Direction2d.Right;
        }

        private ActivityType GetNewActivity(bool destinationReached) =>
            destinationReached ? Enums.ActivityType.Stay : Enums.ActivityType.Move;

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