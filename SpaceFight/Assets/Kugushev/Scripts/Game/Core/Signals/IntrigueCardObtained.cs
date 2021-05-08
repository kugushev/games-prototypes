using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Game.Core.Signals
{
    public class IntrigueCardObtained : Poolable<Intrigue>
    {
        private readonly IntrigueCard.Factory _intrigueCardFactory;

        public IntrigueCardObtained(IntrigueCard.Factory intrigueCardFactory)
        {
            _intrigueCardFactory = intrigueCardFactory;
        }

        public IntrigueCard CreateCard() => _intrigueCardFactory.Create(Parameter);

        public class Factory : PlaceholderFactory<Intrigue, IntrigueCardObtained>
        {
        }
    }
}