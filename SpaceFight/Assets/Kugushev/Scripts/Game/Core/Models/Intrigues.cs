using Kugushev.Scripts.Game.Core.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class Intrigues
    {
        private readonly ReactiveCollection<IntrigueCard> _intrigues = new ReactiveCollection<IntrigueCard>();

        public IReadOnlyReactiveCollection<IntrigueCard> IntrigueCards => _intrigues;

        // todo: execute it via Signal
        internal void HandleCardUsed(IntrigueCard card)
        {
            _intrigues.Remove(card);
        }

        // todo: by signal, it should be defined and bind here but fired in Mission
        internal void HandleCardObtained(IntrigueCard card)
        {
            _intrigues.Add(card);
        }
    }
}