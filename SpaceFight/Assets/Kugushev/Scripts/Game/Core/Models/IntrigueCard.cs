using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class IntrigueCard : Poolable<Intrigue>
    {
        public Intrigue Intrigue => Parameter;

        public class Factory : PlaceholderFactory<Intrigue, IntrigueCard>
        {
        }
    }
}