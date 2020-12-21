using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Utils.ComponentInjection;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Behaviors;
using Kugushev.Scripts.Game.Services;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.Characters.Abstractions
{
    public abstract class Character : Model, IMovable
    {
        #region IMovable

        [ComponentInjection]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local", Justification = "Use Component Injection")]
        public IPathfindingService PathfindingService { get; private set; }
        public Position? Destination { get; set; }

        #endregion

    }
}