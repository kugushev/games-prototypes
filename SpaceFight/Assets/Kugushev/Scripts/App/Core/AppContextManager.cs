using Kugushev.Scripts.App.Core.ContextManagement;
using Kugushev.Scripts.App.Core.ValueObjects;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Zenject;

namespace Kugushev.Scripts.App.Core
{
    internal class AppContextManager : AbstractContextManager
    {
        [Inject] private MainMenuState _mainMenu = default!;

        [Inject] private GameState _game = default!;
        [Inject] private SignaledTransition<GameParameters> _onNewGameSignal = default!;

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
                    _onNewGameSignal.TransitTo(_game)
                }
            }
        };
    }
}