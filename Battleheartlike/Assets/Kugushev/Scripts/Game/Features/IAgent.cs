using Kugushev.Scripts.Game.AI.BehaviorTree.Abstractions;

namespace Kugushev.Scripts.Game.Features
{
    public interface IAgent
    {
        void Sense();
        IBehaviorTreeTask Think();
        bool Act(IBehaviorTreeTask root);
    }
}