using Kugushev.Scripts.Game.Core.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Game.Core.Models
{
    public interface IIntrigues
    {
        IReadOnlyReactiveCollection<IntrigueCard> IntrigueCards { get; }
    }

    internal class Intrigues : IIntrigues
    {
        private readonly ReactiveCollection<IntrigueCard> _intrigues = new ReactiveCollection<IntrigueCard>();

        public IReadOnlyReactiveCollection<IntrigueCard> IntrigueCards => _intrigues;

        internal void HandleCardApplied(IntrigueCard card) => _intrigues.Remove(card);

        internal void ObtainCard(IntrigueCard card) => _intrigues.Add(card);
    }
}