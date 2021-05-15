using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Kugushev.Scripts.Mission.Core.ContextManagement;
using Zenject;

namespace Kugushev.Scripts.Mission.Core
{
    public class MissionContextManager : AbstractContextManager
    {
        [Inject] private MissionDataInitializer _onMissionDataInitialized = default!;
        [Inject] private BriefingState _briefing = default!;

        protected override Transitions ComposeStateMachine() => new Transitions
        {
            {
                Entry, new[]
                {
                    _onMissionDataInitialized.TransitTo(_briefing)
                }
            }
        };
    }
}