using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Kugushev.Scripts.Game.Core.ContextManagement;
using Kugushev.Scripts.Game.Core.ContextManagement.Parameters;
using Zenject;
using PoliticsState = Kugushev.Scripts.Game.Core.ContextManagement.PoliticsState;

namespace Kugushev.Scripts.Game.Core
{
    internal class GameContextManager : AbstractContextManager
    {
        [Inject] private GameDataInitializedTransition _onGameDataInitialized = default!;
        [Inject] private PoliticsState _politics = default!;
        [Inject] private IParameterizedTransition<CampaignParameters> _onStartCampaign = default!;
        [Inject] private CampaignState _campaign = default!;
        [Inject] private IParameterizedTransition<RevolutionParameters> _onRevolutionDeclared = default!;
        [Inject] private RevolutionState _revolution = default!;
        [Inject] private IParameterizedTransition<GameExitParameters> _onExit = default!;
        [Inject] private ExitState<GameExitParameters> _exit = default!;

        protected override Transitions ComposeStateMachine() => new Transitions
        {
            {
                Entry, new[]
                {
                    _onGameDataInitialized.TransitTo(_politics)
                }
            },
            {
                _politics, new ITransitionRecord[]
                {
                    _onStartCampaign.TransitTo(_campaign),
                    _onRevolutionDeclared.TransitTo(_revolution)
                }
            },
            {
                _revolution, new[]
                {
                    _onExit.TransitTo(_exit)
                }
            }

            /*               {
                    campaignState, new[]
                    {
                        new TransitionRecordOld(onCampaignExitTransition, politicsState)
                    }
                }
             */
        };
    }
}