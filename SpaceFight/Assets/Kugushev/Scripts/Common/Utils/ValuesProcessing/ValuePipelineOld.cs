using System.Collections.Generic;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Common.Utils.ValuesProcessing
{
    public class ValuePipelineOld<T> : PoolableOld<int>, IValuePipeline<T>
    {
        private readonly List<IPercentPerk<T>> _percentPerks = new List<IPercentPerk<T>>(16);
        private readonly List<IMultiplierPerk<T>> _multiplierPerks = new List<IMultiplierPerk<T>>(16);

        public ValuePipelineOld(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public void AddPerk(IMultiplierPerk<T> perk) => _multiplierPerks.Add(perk);
        public void AddPerk(IPercentPerk<T> perk) => _percentPerks.Add(perk);

        public float Calculate(float value, T criteria)
        {
            var percentageAmount = GetPercentageAmount(criteria);
            var multiplier = GetMultiplier(criteria);

            var result = value * percentageAmount * multiplier;

            return result;
        }

        private float GetPercentageAmount(T criteria)
        {
            var percentageAmount = 1f;
            foreach (var perk in _percentPerks)
            {
                var percentage = perk.GetPercentage(criteria);
                percentageAmount += percentage.Amount;
            }

            return percentageAmount;
        }

        private float GetMultiplier(T criteria)
        {
            var multiplier = 1f;
            foreach (var perk in _multiplierPerks)
            {
                var m = perk.GetMultiplier(criteria);
                if (m != null)
                    multiplier *= m.Value;
            }

            return multiplier;
        }

        protected override void OnRestore(int state) => _percentPerks.Clear();

        protected override void OnClear(int state) => _percentPerks.Clear();
    }
}