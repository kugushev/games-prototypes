using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core.Signals
{
    public class ObtainPerk : Poolable<PerkInfo>
    {
        public PerkInfo Perk => Parameter;

        public class Factory : PlaceholderFactory<PerkInfo, ObtainPerk>
        {
        }
    }
}