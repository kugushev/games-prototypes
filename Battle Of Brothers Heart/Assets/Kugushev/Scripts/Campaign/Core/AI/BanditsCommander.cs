using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Kugushev.Scripts.Campaign.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Models;
using UnityEngine;
using Zenject;
using static Kugushev.Scripts.Campaign.Core.CampaignConstants.AI;

namespace Kugushev.Scripts.Campaign.Core.AI
{
    public class BanditsCommander : IFixedTickable
    {
        private readonly Wayfarers _wayfarers;
        private readonly OrderAttackPlayer.Factory _orderAttackPlayerFactory;
        private readonly OrderMove.Factory _orderMoveFactory;

        public BanditsCommander(Wayfarers wayfarers, OrderAttackPlayer.Factory orderAttackPlayerFactory,
            OrderMove.Factory orderMoveFactory)
        {
            _wayfarers = wayfarers;
            _orderAttackPlayerFactory = orderAttackPlayerFactory;
            _orderMoveFactory = orderMoveFactory;
        }

        void IFixedTickable.FixedTick()
        {
            for (int i = 0; i < _wayfarers.Bandits.Count; i++)
            {
                var bandit = _wayfarers.Bandits[i];

                if (TryReturnToCity(bandit))
                    continue;

                if (TryOrderAttackPlayer(bandit))
                    continue;

                if (TryOrderMove(bandit))
                    continue;
            }
        }

        private bool TryReturnToCity(BanditWayfarer bandit)
        {
            var distance = Vector2.Distance(bandit.Position.Value.Vector, bandit.City.Position.Vector);
            if (distance >= BanditsCityRadius)
            {
                // todo: cut vector by distance
                bandit.CurrentOrder = _orderMoveFactory.Create(bandit.City.Position);
                return true;
            }

            return false;
        }

        private bool TryOrderAttackPlayer(BanditWayfarer bandit)
        {
            if (bandit.CurrentOrder is OrderMove move && move.Target == bandit.City.Position)
                return false;

            var distance = Vector2.Distance(bandit.Position.Value.Vector, _wayfarers.Player.Position.Value.Vector);
            if (distance <= BanditsAggroDistance)
            {
                bandit.CurrentOrder = _orderAttackPlayerFactory.Create(_wayfarers.Player);
                return true;
            }

            return false;
        }

        private bool TryOrderMove(BanditWayfarer bandit)
        {
            if (bandit.CurrentOrder == null)
            {
                var position = CreateRandomStep(bandit);
                bandit.CurrentOrder = _orderMoveFactory.Create(position);
                return true;
            }

            return false;
        }

        private static Position CreateRandomStep(BanditWayfarer bandit)
        {
            float stepDistance = Random.Range(BanditsStepDistanceMin, BanditsStepDistanceMax);
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;

            float x = stepDistance * Mathf.Cos(angle);
            float y = stepDistance * Mathf.Sin(angle);

            return new Position(bandit.Position.Value.Vector + new Vector2(x, y));
        }
    }
}