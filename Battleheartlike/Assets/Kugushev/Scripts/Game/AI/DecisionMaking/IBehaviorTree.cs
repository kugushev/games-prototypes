using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Game.AI.DecisionMaking.Behaviors.Abstractions;

namespace Kugushev.Scripts.Game.AI.DecisionMaking
{
    public interface IBehaviorTree: IDisposable
    {
        void SetRootTask(IBehaviorTreeTask task);

        UniTask RunLoop();
    }
}