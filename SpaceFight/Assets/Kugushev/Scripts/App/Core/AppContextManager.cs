using Kugushev.Scripts.App.Core.ContextManagement;
using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Zenject;

namespace Kugushev.Scripts.App.Core
{
    internal class AppContextManager : AbstractContextManager
    {
        [Inject] private MainMenuState _mainMenu = default!;

        [Inject] private GameState _game = default!;
        [Inject] private IParameterizedTransition<GameParameters> _onNewGame = default!;
        [Inject] private IParameterizedTransition<GameExitParameters> _onGameExit = default!;

        protected override Transitions ComposeStateMachine() => new Transitions
        {
            {
                Entry, new[]
                {
                    Immediate.TransitTo(_mainMenu)
                }
            },
            {
                _mainMenu, new[]
                {
                    _onNewGame.TransitTo(_game)
                }
            },
            {
                _game, new[]
                {
                    _onGameExit.TransitTo(_mainMenu)
                }
            }
        };
    }
}