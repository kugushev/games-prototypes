using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Game.AI.DecisionMaking.Behaviors.Abstractions
{
    public abstract class BehaviorTreeTask<TState> : Poolable<TState>, IBehaviorTreeTask
        where TState : struct
    {
        private bool _canceled;

        protected BehaviorTreeTask(ObjectsPool objectsPool) : base(objectsPool)
        {
        }
        public abstract UniTask<bool> RunAsync();
        protected UniTask AwaitOrCancel(UniTask task) => UniTask.WhenAny(task, UniTask.WaitUntil(IsCanceled));
        protected override void OnRestore() => _canceled = false;
        public void Cancel() => _canceled = true;
        private bool IsCanceled() => _canceled;
    }
}