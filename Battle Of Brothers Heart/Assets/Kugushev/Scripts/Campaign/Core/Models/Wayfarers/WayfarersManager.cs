using Kugushev.Scripts.Common.Core.Controllers;
using Kugushev.Scripts.Game.Core.Models.AI.Orders;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class WayfarersManager
    {
        public WayfarersManager(InputController inputController, OrderMove.Factory orderMoveFactory)
        {
            Player = new PlayerWayfarer(new Position(new Vector2(0, 0)), inputController, orderMoveFactory);
        }

        public PlayerWayfarer Player { get; }
    }
}