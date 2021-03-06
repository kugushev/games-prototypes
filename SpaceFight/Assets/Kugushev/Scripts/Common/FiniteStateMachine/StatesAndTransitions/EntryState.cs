using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.FiniteStateMachine.StatesAndTransitions
{
    public class EntryState : IState
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