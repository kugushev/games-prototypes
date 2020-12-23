using System;
using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Game.AI.DecisionMaking.Behaviors.Abstractions
{
    public interface IBehaviorTreeTask: IDisposable
    {
        UniTask<bool> RunAsync();
        void Cancel();
    }
}