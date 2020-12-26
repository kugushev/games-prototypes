using JetBrains.Annotations;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Features;
using UnityEngine;

namespace Kugushev.Scripts.Game.Services
{
    [CreateAssetMenu(fileName = "InteractionsService", menuName = "Game/InteractionsService", order = 0)]
    public class PlayerInteractionsService : ScriptableObject
    {
        public bool TryFindAndSetBehavior(ICharacter active, [CanBeNull] IInteractionParty passive, in Position target)
        {
            if (!ReferenceEquals(passive, null))
            {
                // todo: use passive
                return false;
            }

            return ExecuteMoveTo(active, in target);
        }

        private bool ExecuteMoveTo(ICharacter active, in Position target)
        {
            var task = active.GetMovementBehavior(in target);
            if (task == null)
                return false;
            
            active.BehaviorTreeManager.SetRootTask(task);
            return true;
        }
    }
}