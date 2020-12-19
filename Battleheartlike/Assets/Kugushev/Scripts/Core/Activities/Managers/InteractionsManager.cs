using System.Collections.Generic;
using Kugushev.Scripts.Common.Pooling;
using Kugushev.Scripts.Core.Activities.Abstractions;
using Kugushev.Scripts.Core.Behaviors;
using Kugushev.Scripts.Core.ValueObjects;
using Kugushev.Scripts.Models.Characters.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Core.Activities.Managers
{
    [CreateAssetMenu(fileName = "InteractionsManager", menuName = "Game/InteractionsManager", order = 0)]
    public class InteractionsManager : ScriptableObject
    {
        [SerializeField] private ObjectsPool objectsPool;

        public bool TryExecuteInteraction(Character active, IReadOnlyList<IInteractable> passives, in Position target)
        {
            if (passives.Count > 0)
            {
                // todo: use passives
                return false;
            }

            using (var interaction = objectsPool.GetObject<MovementActivity, ActivityState<IMovable, Position>>(
                new ActivityState<IMovable, Position>(active, target)))
            {
                var objective = interaction.Act();
                if (objective != null) 
                    active.AppendObjective(objective);
                return objective != null;
            }
        }
    }
}