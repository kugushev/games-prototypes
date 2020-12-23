using JetBrains.Annotations;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.AI.DecisionMaking.Behaviors;
using Kugushev.Scripts.Game.Features;
using Kugushev.Scripts.Game.Models.Characters.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.Services
{
    [CreateAssetMenu(fileName = "InteractionsService", menuName = "Game/InteractionsService", order = 0)]
    public class InteractionsService : ScriptableObject
    {
        [SerializeField] private ObjectsPool pool;

        public void ExecuteInteraction(Character active, [CanBeNull] Character passive, in Position target)
        {
            if (!ReferenceEquals(passive, null))
            {
                // todo: use passive
                return;
            }

            ExecuteMoveTo(active, in target);
        }

        private void ExecuteMoveTo(Character character, in Position target)
        {
            character.BehaviorTree.SetRootTask(pool.GetObject<MoveToTask, MoveToTask.State>(
                new MoveToTask.State(character, target))
            );
        }
    }
}