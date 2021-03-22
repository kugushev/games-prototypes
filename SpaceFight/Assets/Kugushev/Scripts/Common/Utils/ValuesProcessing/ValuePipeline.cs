using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils.Pooling;
using UnityEngine;

namespace Kugushev.Scripts.Common.Utils.ValuesProcessing
{
    [Serializable]
    public class ValuePipeline<T> : Poolable<int>, IValuePipeline<T>
    {
        [SerializeReference] private List<IPercentPerk<T>> percentPerks = new List<IPercentPerk<T>>(16);
        [SerializeReference] private List<IMultiplierPerk<T>> multiplierPerks = new List<IMultiplierPerk<T>>(16);

        public ValuePipeline(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public void AddPerk(IMultiplierPerk<T> perk) => multiplierPerks.Add(perk);
        public void AddPerk(IPercentPerk<T> perk) => percentPerks.Add(perk);

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
            foreach (var perk in percentPerks)
            {
                var percentage = perk.GetPercentage(criteria);
                percentageAmount += percentage.Amount;
            }

            return percentageAmount;
        }

        private float GetMultiplier(T criteria)
        {
            var multiplier = 1f;
            foreach (var perk in multiplierPerks)
            {
                var m = perk.GetMultiplier(criteria);
                if (m != null)
                    multiplier *= m.Value;
            }

            return multiplier;
        }

        protected override void OnRestore(int state) => percentPerks.Clear();

        protected override void OnClear(int state) => percentPerks.Clear();
    }
}