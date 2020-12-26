using Kugushev.Scripts.Game.AI.BehaviorTree;

namespace Kugushev.Scripts.Game.Features
{
    public interface IActive
    {
        IBehaviorTreeManager BehaviorTreeManager { get; }
    }
}