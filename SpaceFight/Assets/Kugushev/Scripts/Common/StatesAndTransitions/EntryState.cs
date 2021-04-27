using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Common.StatesAndTransitions
{
    public class EntryState : IUnparameterizedState
    {
        public static EntryState Instance { get; } = new EntryState();

        private EntryState()
        {
        }

        public bool CanEnter => true;
        public UniTask OnEnterAsync() => UniTask.CompletedTask;

        public void OnUpdate(float deltaTime)
        {
        }

        public UniTask OnExitAsync() => UniTask.CompletedTask;
    }
}