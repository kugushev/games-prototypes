using Kugushev.Scripts.Game.AI.DecisionMaking.Behaviors.Abstractions;

namespace Kugushev.Scripts.Game.Features
{
    public interface IAgent
    {
        void Sense();
        IBehaviorTreeTask Think();
        bool Act(IBehaviorTreeTask root);
    }
}