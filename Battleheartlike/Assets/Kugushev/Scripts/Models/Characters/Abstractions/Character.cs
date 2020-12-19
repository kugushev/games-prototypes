using System;
using Kugushev.Scripts.Models.Behaviors;
using Kugushev.Scripts.Models.Services;
using Kugushev.Scripts.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Models.Characters.Abstractions
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