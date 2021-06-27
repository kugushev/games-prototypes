﻿using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Game.Core.Models;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core.ValueObjects.Orders
{
    public class OrderVisitCity : OrderInteract<City>
    {
        public class Factory : PlaceholderFactory<City, OrderVisitCity>
        {
        }
    }
}