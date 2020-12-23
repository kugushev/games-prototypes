using Kugushev.Scripts.Game.AI.DecisionMaking;

namespace Kugushev.Scripts.Game.Features
{
    public interface IActive
    {
        IBehaviorTree BehaviorTree { get; }
    }
}