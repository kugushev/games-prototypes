using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Tests.Utils
{
    public class SingletonState: IState
    {
        public static SingletonState Instance { get; } = new SingletonState();
        
        private SingletonState()
        {
            
        }
        
        public bool Entered { get; private set; }

        public void Reset() => Entered = false;
        
        UniTask IState.OnEnterAsync()
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