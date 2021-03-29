using System.Collections.Generic;
using JetBrains.Annotations;
using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Game.Models
{
    public class Parliament : Poolable<Parliament.State>
    {
        public struct State
        {
            public Politician SelectedPolitician;
        }

        public Parliament(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        private readonly List<Politician> _politicians = new List<Politician>(9);

        public void AddPolitician(Politician politician) => _politicians.Add(politician);

        public IReadOnlyList<Politician> Politicians => _politicians;

        [CanBeNull]
        public Politician SelectedPolitician
        {
            get => ObjectState.SelectedPolitician;
            set => ObjectState.SelectedPolitician = value;
        }

        protected override void OnRestore(State state)
        {
            foreach (var politician in _politicians)
                politician.Dispose();
            _politicians.Clear();
        }

        protected override void OnClear(State state) => _politicians.Clear();
    }
}