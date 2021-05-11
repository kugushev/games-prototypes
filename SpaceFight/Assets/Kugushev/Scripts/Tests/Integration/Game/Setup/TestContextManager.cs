using Kugushev.Scripts.App.Core.ContextManagement;
using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Zenject;

namespace Kugushev.Scripts.Tests.Integration.Game.Setup
{
    public class TestContextManager : AbstractContextManager
    {
        [Inject] private GameState _gameState = default!;

        protected override Transitions ComposeStateMachine()
        {
            return new Transitions
            {
                {
                    Entry, new[]
                    {
                        SingletonTransition<GameParameters>.Instance.TransitTo(_gameState)
                    }
                }
            };
        }
    }
}