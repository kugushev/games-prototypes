using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Zenject;

namespace Kugushev.Scripts.Game.Core.ContextManagement
{
    internal class GameContextManager : AbstractContextManager
    {
        [Inject] private GameStoreInitializedTransition _onGameStoreInitialized = default!;
        [Inject] private PoliticsState _politicsState = default!;

        protected override Transitions ComposeStateMachine() => new Transitions
        {
            {
                Entry, new[]
                {
                    _onGameStoreInitialized.TransitTo(_politicsState)
                }
            }
        };
    }
}