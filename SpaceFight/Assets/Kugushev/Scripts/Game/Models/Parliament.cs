using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Game.Models
{
    public class Parliament : Poolable<int>
    {
        public Parliament(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        private readonly List<Politician> _politicians = new List<Politician>(9);

        public void AddPolitician(Politician politician) => _politicians.Add(politician);

        public IReadOnlyList<Politician> Politicians => _politicians;

        protected override void OnRestore(int state) => _politicians.Clear();
        protected override void OnClear(int state) => _politicians.Clear();
    }
}