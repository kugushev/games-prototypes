using Kugushev.Scripts.Game.Core.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Game.Core.Models
{
    public interface IIntrigues
    {
        IReadOnlyReactiveCollection<IntrigueCard> IntrigueCards { get; }
        void ObtainCard(Intrigue intrigue);
    }

    internal class Intrigues : IIntrigues
    {
        private readonly IntrigueCard.Factory _intrigueCardFactory;

        public Intrigues(IntrigueCard.Factory intrigueCardFactory)
        {
            _intrigueCardFactory = intrigueCardFactory;
        }

        private readonly ReactiveCollection<IntrigueCard> _intrigues = new ReactiveCollection<IntrigueCard>();

        public IReadOnlyReactiveCollection<IntrigueCard> IntrigueCards => _intrigues;

        void IIntrigues.ObtainCard(Intrigue intrigue)
        {
            var card = _intrigueCardFactory.Create(intrigue);
            _intrigues.Add(card);
        }

        internal void HandleCardApplied(IntrigueCard card) => _intrigues.Remove(card);
    }
}