using System.Threading;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Game.AI.BehaviorTree.Abstractions
{
    public abstract class BehaviorTreeTask<TState> : Poolable<TState>, IBehaviorTreeTask
        where TState : struct
    {

        protected BehaviorTreeTask(ObjectsPool objectsPool) : base(objectsPool)
        {
        }
        public abstract UniTask<bool> RunAsync(CancellationToken cancellationToken);
    }
}