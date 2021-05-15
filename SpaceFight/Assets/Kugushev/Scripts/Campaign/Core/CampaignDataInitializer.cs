using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core
{
    public class CampaignDataInitializer : IInitializable, ITransition
    {
        public bool ToTransition { get; private set; }

        public void Initialize()
        {
        }
    }
}