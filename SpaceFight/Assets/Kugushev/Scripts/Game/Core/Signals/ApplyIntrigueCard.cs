using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Game.Core.Signals
{
    public class ApplyIntrigueCard : Poolable<(IntrigueCard, IPolitician)>
    {
        public IntrigueCard Card => Parameter.Item1;

        public void ExecuteApply()
        {
            var politician = (Politician) Parameter.Item2;
            politician.ApplyIntrigue(Card.Intrigue);
        }

        public class Factory : PlaceholderFactory<(IntrigueCard, IPolitician), ApplyIntrigueCard>
        {
        }
    }
}