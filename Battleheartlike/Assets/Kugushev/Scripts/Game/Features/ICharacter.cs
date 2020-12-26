using JetBrains.Annotations;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.AI.BehaviorTree.Abstractions;

namespace Kugushev.Scripts.Game.Features
{
    public interface ICharacter: IActive
    {
        [CanBeNull] IBehaviorTreeTask GetMovementBehavior(in Position target);
        [CanBeNull] IBehaviorTreeTask GetAttackBehavior();
        [CanBeNull] IBehaviorTreeTask GetAssistBehavior();
    }
}