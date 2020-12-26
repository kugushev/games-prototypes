using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Game.AI.BehaviorTree.Abstractions;

namespace Kugushev.Scripts.Game.AI.BehaviorTree
{
    public interface IBehaviorTreeManager: IDisposable
    {
        void SetRootTask(IBehaviorTreeTask task);

        UniTask RunLoop();
    }
}