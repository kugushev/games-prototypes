using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Behaviors;
using Kugushev.Scripts.Game.Services;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.Characters.Abstractions
{
    public abstract class Character : ScriptableObject, IMovable
    {
        #region IMovable

        // todo: fix it with more safe apprach
        public IPathfindingService PathfindingService { get; set; }
        public Position? Destination { get; set; }

        #endregion

    }
}