using Kugushev.Scripts.Campaign.Core.ContextManagement;
using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Kugushev.Scripts.Game.Core.ContextManagement.Parameters;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core
{
    public class CampaignContextManager : AbstractContextManager
    {
        [Inject] private CampaignDataInitializer _onDataInitialized = default!;
        [Inject] private MissionSelectionState _missionSelection = default!;
        [Inject] private IParameterizedTransition<MissionParameters> _onStartMission = default!;
        [Inject] private MissionState _mission = default!;
        [Inject] private IParameterizedTransition<CampaignExitParameters> _onExit = default!;
        [Inject] private ExitState<CampaignExitParameters> _exit = default!;

        protected override Transitions ComposeStateMachine() => new Transitions
        {
            {
                Entry, new[]
                {
                    _onDataInitialized.TransitTo(_missionSelection)
                }
            },
            {
                _missionSelection, new ITransitionRecord[]
                {
                    _onStartMission.TransitTo(_mission),
                    _onExit.TransitTo(_exit)
                }
            }
            // {
            // missionState, new[]
            // {
            //     new TransitionRecordOld(onMissionExitTransition, missionSelectionState)
            // }
            // },
        };
    }
}