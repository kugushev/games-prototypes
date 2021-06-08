using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Core.Battle.Models.Squad
{
    public abstract class BaseSquad
    {
        public abstract IReadOnlyList<BaseUnit> BaseUnits { get; }

        public void ProcessOrders(DeltaTime delta)
        {
            foreach (var unit in BaseUnits)
            {
                unit.ProcessCurrentOrder(delta);
            }
        }
    }

    public abstract class BaseSquad<TUnit> : BaseSquad
        where TUnit : BaseUnit
    {
        protected readonly ReactiveCollection<TUnit> Units = new ReactiveCollection<TUnit>();

        public override IReadOnlyList<BaseUnit> BaseUnits => Units;
    }
}