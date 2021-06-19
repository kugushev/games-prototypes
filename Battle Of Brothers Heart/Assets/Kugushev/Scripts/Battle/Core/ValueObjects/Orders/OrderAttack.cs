using System;
using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.Models.Units;
using Zenject;

namespace Kugushev.Scripts.Battle.Core.ValueObjects.Orders
{
    public class OrderAttack : OrderInteract<BaseUnit>
    {
        public class Factory : PlaceholderFactory<BaseUnit, OrderAttack>
        {
        }
    }
}