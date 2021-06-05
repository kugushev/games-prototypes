using System;
using Kugushev.Scripts.Core.Battle.Enums;
using Kugushev.Scripts.Core.Battle.Interfaces;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Core.Battle.Models
{
    public class SquadUnit
    {
        private readonly ReactiveProperty<Position> _position = new ReactiveProperty<Position>();

        private readonly ReactiveProperty<UnitDirection> _direction =
            new ReactiveProperty<UnitDirection>(UnitDirection.Down);

        private readonly ReactiveProperty<UnitActivity> _activity =
            new ReactiveProperty<UnitActivity>(UnitActivity.Stay);

        public IReadOnlyReactiveProperty<Position> Position => _position;
        public IReadOnlyReactiveProperty<UnitDirection> Direction => _direction;
        public IReadOnlyReactiveProperty<UnitActivity> Activity => _activity;
        public IOrder? CurrentOrder { get; set; }

        internal void ProcessCurrentOrder(DeltaTime delta)
        {
            if (CurrentOrder is { })
            {
                OrderProcessingStatus processingStatus = OrderProcessingStatus.None;

                if (CurrentOrder is OrderMove orderMove)
                    processingStatus = Process(orderMove, delta);

                if (processingStatus == OrderProcessingStatus.Completed)
                    CurrentOrder = null;
            }
        }

        private OrderProcessingStatus Process(OrderMove order, DeltaTime delta)
        {
            _position.Value = GetNewPosition(order, delta);

            var direction = order.Target.Vector - _position.Value.Vector;
            _direction.Value = GetNewDirection(direction);

            var inProgress = direction.sqrMagnitude > BattleConstants.UnitToTargetEpsilon;
            _activity.Value = GetNewActivity(inProgress);

            return inProgress ? OrderProcessingStatus.InProgress : OrderProcessingStatus.Completed;
        }

        private Position GetNewPosition(OrderMove order, DeltaTime delta)
        {
            var targetVector = order.Target.Vector;
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

        private UnitActivity GetNewActivity(bool inProgress) => inProgress ? UnitActivity.Move : UnitActivity.Stay;
    }
}