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
        [Inject] private GameStoreInitializedTransition _onGameStoreInitialized = default!;
        [Inject] private PoliticsState _politicsState = default!;
        [Inject] private SignaledTransition<CampaignParameters> _onStartCampaign = default!;
        [Inject] private CampaignState _campaignState = default!;
        [Inject] private SignaledTransition<RevolutionParameters> _onRevolutionDeclared = default!;
        [Inject] private RevolutionState _revolutionState = default!;

        protected override Transitions ComposeStateMachine() => new Transitions
        {
            {
                Entry, new[]
                {
                    _onGameStoreInitialized.TransitTo(_politicsState)
                }
            },
            {
                _politicsState, new ITransitionRecord[]
                {
                    _onStartCampaign.TransitTo(_campaignState),
                    _onRevolutionDeclared.TransitTo(_revolutionState)
                }
            }
            
            /*               {
                    campaignState, new[]
                    {
                        new TransitionRecordOld(onCampaignExitTransition, politicsState)
                    }
                },
                {
                    revolutionState, new[]
                    {
                        new TransitionRecordOld(toMainMenuTransition, gameExitState)
                    }
                }
             */

        };
    }
}