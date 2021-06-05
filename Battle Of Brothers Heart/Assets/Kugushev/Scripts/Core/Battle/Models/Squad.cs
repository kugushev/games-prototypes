using System;
using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Core.Battle.Models
{
    public class Squad
    {
        private readonly List<SquadUnit> _units = new List<SquadUnit>(4);

        // todo: stop calling in the real cases
        public void Add(SquadUnit unit) => _units.Add(unit);
        
        public void ProcessOrders(DeltaTime delta)
        {
            foreach (var unit in _units)
            {
                unit.ProcessCurrentOrder(delta);
            }
        }
    }
}