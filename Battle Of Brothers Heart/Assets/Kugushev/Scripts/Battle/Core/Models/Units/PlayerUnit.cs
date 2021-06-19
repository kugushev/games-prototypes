using JetBrains.Annotations;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Battle.Core.Models.Units
{
    public class PlayerUnit : BaseUnit
    {
        private readonly ReactiveProperty<bool> _selected = new ReactiveProperty<bool>();

        public PlayerUnit(Position position, [NotNull] Battlefield battlefield) : base(position, battlefield)
        {
        }

        public IReadOnlyReactiveProperty<bool> Selected => _selected;

        internal void Select() => _selected.Value = true;

        internal void Deselect() => _selected.Value = false;
    }
}