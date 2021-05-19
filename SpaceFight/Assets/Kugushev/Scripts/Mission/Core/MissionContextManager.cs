using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Kugushev.Scripts.Mission.Core.ContextManagement;
using Kugushev.Scripts.Mission.Core.ContextManagement.Transitions;
using Zenject;

namespace Kugushev.Scripts.Mission.Core
{
    public class MissionContextManager : AbstractContextManager
    {
        [Inject] private BriefingState _briefing = default!;
        [Inject] private ExecutionState _execution = default!;

        [Inject] private MissionDataInitializer _onMissionDataInitialized = default!;
        [Inject] private ToExecutionTransition _toExecutionTransition = default!;

        protected override Transitions ComposeStateMachine() => new Transitions
        {
            {
                Entry, new[]
                {
                    _onMissionDataInitialized.TransitTo(_briefing)
                }
            },
            {
                _briefing, new[]
                {
                    _toExecutionTransition.TransitTo(_execution)
                }
            }
        };
    }
}