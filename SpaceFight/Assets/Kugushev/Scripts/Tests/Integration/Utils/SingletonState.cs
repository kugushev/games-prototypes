using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Tests.Integration.Utils
{
    public class SingletonState : IUnparameterizedState
    {
        public static SingletonState Instance { get; } = new SingletonState();

        private SingletonState()
        {
        }

        public bool Entered { get; private set; }

        public void Reset() => Entered = false;

        UniTask IUnparameterizedState.OnEnterAsync()
        {
            Entered = true;
            return UniTask.CompletedTask;
        }

        void IState.OnUpdate(float deltaTime)
        {
        }

        UniTask IState.OnExitAsync()
        {
            Entered = false;
            return UniTask.CompletedTask;
        }
    }
}