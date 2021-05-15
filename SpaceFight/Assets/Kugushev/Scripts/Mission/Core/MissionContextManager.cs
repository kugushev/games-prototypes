using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Zenject;

namespace Kugushev.Scripts.Mission.Core
{
    public class MissionContextManager : AbstractContextManager
    {
        [Inject] private MissionDataInitializer _onMissionDataInitialized = default!;

        protected override Transitions ComposeStateMachine() => new Transitions
        {
        };
    }
}