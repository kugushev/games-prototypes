using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine
{
    public abstract class BaseState<TModel> : IState
    {
        protected readonly TModel Model;

        protected BaseState(TModel model)
        {
            Model = model;
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