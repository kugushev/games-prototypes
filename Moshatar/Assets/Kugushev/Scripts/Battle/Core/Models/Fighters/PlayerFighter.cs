using Kugushev.Scripts.Battle.Core.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Battle.Core.Models.Fighters
{
    public class PlayerFighter : BaseFighter
    {
        private readonly ReactiveProperty<bool> _selected = new ReactiveProperty<bool>();

        public PlayerFighter(Position battlefieldPosition, Character character, Battlefield battlefield)
            : base(battlefieldPosition, character, battlefield)
        {
        }

        public IReadOnlyReactiveProperty<bool> Selected => _selected;

        internal void Select() => _selected.Value = true;

        internal void Deselect() => _selected.Value = false;
    }
}