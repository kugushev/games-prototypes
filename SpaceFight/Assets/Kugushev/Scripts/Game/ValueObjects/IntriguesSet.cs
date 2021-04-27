using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Models;
using Zenject;

namespace Kugushev.Scripts.Game.ValueObjects
{
    public class IntriguesSet : SelfDespawning
    {
        private readonly List<IntrigueRecord> _intrigues = new List<IntrigueRecord>(16);

        public class Pool : MemoryPool<IntriguesSet>
        {
            protected override void Reinitialize(IntriguesSet item) => item.PoolReference = this;

            protected override void OnDespawned(IntriguesSet item) => item.Intrigues.Clear();
        }

        public List<IntrigueRecord> Intrigues => _intrigues;
    }
}