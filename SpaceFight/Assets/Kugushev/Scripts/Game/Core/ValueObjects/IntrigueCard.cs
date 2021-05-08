using Kugushev.Scripts.Common.Utils.Pooling;
using Zenject;

namespace Kugushev.Scripts.Game.Core.ValueObjects
{
    public class IntrigueCard : Poolable<Intrigue>
    {
        public Intrigue Intrigue => Parameter;

        public class Factory : PlaceholderFactory<Intrigue, IntrigueCard>
        {
        }
    }
}