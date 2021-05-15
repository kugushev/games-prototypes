using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine
{
    public abstract class BaseState<TModel> : IUnparameterizedState
    {
        protected readonly TModel ModelOld;

        protected BaseState(TModel modelOld)
        {
            ModelOld = modelOld;
        }

        public virtual UniTask OnEnterAsync()
        {
            return UniTask.CompletedTask;
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }

        public virtual UniTask OnExitAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}