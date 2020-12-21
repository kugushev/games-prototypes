using JetBrains.Annotations;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Behaviors;
using Kugushev.Scripts.Game.Models.Characters.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.Services
{
    [CreateAssetMenu(fileName = "InteractionsService", menuName = "Game/InteractionsService", order = 0)]
    public class InteractionsService : ScriptableObject
    {
        public bool TryExecuteInteraction(PlayableCharacter active, [CanBeNull] Character passive, in Position target)
        {
            if (!ReferenceEquals(passive, null))
            {
                // todo: use passive
                return false;
            }
            
            return ExecuteMoveTo(active, in target);
        }

        private bool ExecuteMoveTo(IMovable movable, in Position target)
        {
            if (movable.PathfindingService.TestDestination(in target))
            {
                movable.Destination = target;
                return true;
            }

            return false;
        }
    }
}