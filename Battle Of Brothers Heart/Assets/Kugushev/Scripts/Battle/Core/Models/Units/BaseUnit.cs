using System;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Helpers;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Interfaces;
using Kugushev.Scripts.Game.Core.Models.AI;
using Kugushev.Scripts.Game.Core.Models.AI.Orders;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Models.Units
{
    public abstract class BaseUnit : ActiveAgent, IInteractable
    {
        private readonly Battlefield _battlefield;
        private DateTime? _interruptionTime;
        private AttackProcessing? _currentAttack;

        private readonly ReactiveProperty<int> _hitPoints = new ReactiveProperty<int>();

        protected BaseUnit(Position position, Battlefield battlefield) : base(position)
        {
            _battlefield = battlefield;
            _hitPoints.Value = BattleConstants.UnitMaxHitPoints;
        }

        public IReadOnlyReactiveProperty<int> HitPoints => _hitPoints;
        public float WeaponRange => BattleConstants.SwordAttackRange;
        public int Damage => BattleConstants.SwordAttackDamage;
        public bool IsDead => Activity.Value == ActivityType.Death;

        public event Action? Attacking;
        public event Action? AttackCanceled;
        public event Action<BaseUnit>? Hurt;
        public event Action? Die;

        #region IInteractable

        Position IInteractable.Position => Position.Value;
        bool IInteractable.IsInteractable => IsDead;

        #endregion

        public void Suffer(int damage, BaseUnit attacker)
        {
            _hitPoints.Value -= damage;

            if (_hitPoints.Value <= 0)
            {
                ActivityImpl.Value = ActivityType.Death;
                _currentAttack = null;
                CurrentOrder = null;
                Die?.Invoke();
                return;
            }

            _currentAttack = null;
            ActivityImpl.Value = ActivityType.Stay;
            _interruptionTime = DateTime.Now;
            Hurt?.Invoke(attacker);
        }

        protected override bool IsActive => !IsDead;
        protected override float InteractionRadius => BattleConstants.SwordAttackRange;
        protected override float Speed => BattleConstants.UnitSpeed;

        protected override bool CanProcessOrder()
        {
            if (_interruptionTime != null)
            {
                if (DateTime.Now < _interruptionTime + BattleConstants.HurtInterruptionTime)
                    return false;

                _interruptionTime = null;
            }

            return true;
        }

        protected override void ProcessInteraction(OrderInteract order)
        {
            switch (_currentAttack?.Status)
            {
                case null:
                    ActivityImpl.Value = ActivityType.Stay;
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
                    var orderAttack = (OrderAttack) order;
                    orderAttack.Victim.Suffer(Damage, this);
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

        protected override bool CheckCollisions(Vector2 movement, float movementDistance, Vector2 target)
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
                            PositionImpl.Value.Vector, movementDistance,
                            unitVector, unitRadius,
                            out var newPoint1, out var newPoint2);

                        switch (solutions)
                        {
                            case 0:
                                Debug.LogError("Wow, no intersections");
                                continue;
                            case 1:
                                EnqueueToBuffer(newPoint1);
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
        }

        protected override void CancelInteraction()
        {
            if (_currentAttack != null)
            {
                _currentAttack = null;
                AttackCanceled?.Invoke();
            }
        }
    }
}