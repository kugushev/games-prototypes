using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Game.AI.BehaviorTree.Abstractions
{
    public interface IBehaviorTreeTask: IDisposable
    {
        UniTask<bool> RunAsync(CancellationToken cancellationToken);
    }
}