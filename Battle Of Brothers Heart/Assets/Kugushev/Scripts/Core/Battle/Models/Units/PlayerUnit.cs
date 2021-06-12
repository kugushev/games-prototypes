using Kugushev.Scripts.Core.Battle.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Core.Battle.Models.Units
{
    public class PlayerUnit : BaseUnit
    {
        private readonly ReactiveProperty<bool> _selected = new ReactiveProperty<bool>();

        public PlayerUnit(Position position) : base(position)
        {
        }

        public IReadOnlyReactiveProperty<bool> Selected => _selected;

        internal void Select() => _selected.Value = true;

        internal void Deselect() => _selected.Value = false;
    }
}