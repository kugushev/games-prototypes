using Kugushev.Scripts.Common.Modes;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Zenject;

namespace Kugushev.Scripts.App.Modes
{
    public class AppModeManager : AbstractModeManager
    {
        [Inject] private MainMenuState _mainMenuState = default!;

        protected override Transitions ComposeStateMachine()
        {
            return new Transitions
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, _mainMenuState)
                    }
                }
            };
        }
    }
}