using Kugushev.Scripts.Campaign.Core.ContextManagement;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core
{
    public class CampaignContextManager : AbstractContextManager
    {
        [Inject] private MissionSelectionState _missionSelection = default!;

        protected override Transitions ComposeStateMachine() => new Transitions
        {
            {
                Entry, new[]
                {
                    Immediate.TransitTo(_missionSelection)
                }
            }
        };
    }
}