using JetBrains.Annotations;
using Kugushev.Scripts.Common.Pooling;
using Kugushev.Scripts.Models.Behaviors;
using Kugushev.Scripts.Models.Characters.Abstractions;
using Kugushev.Scripts.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Models.Managers
{
    [CreateAssetMenu(fileName = "PlayableCharactersManager", menuName = "Game/PlayableCharactersManager", order = 0)]
    public class PlayableCharactersManager : ScriptableObject
    {
        [SerializeField] private ObjectsPool objectsPool;
        
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