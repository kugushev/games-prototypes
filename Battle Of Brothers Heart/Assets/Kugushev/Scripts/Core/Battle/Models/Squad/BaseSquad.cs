using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Core.Battle.Models.Squad
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