using System.Collections.Generic;
using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Battle.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.Models.Squad
{
    public abstract class BaseSquad
    {
        protected abstract IReadOnlyList<BaseUnit> BaseUnits { get; }

        public void ProcessOrders(DeltaTime delta)
        {
            foreach (var unit in BaseUnits)
            {
                unit.ProcessCurrentOrder(delta);
            }
        }
    }
}