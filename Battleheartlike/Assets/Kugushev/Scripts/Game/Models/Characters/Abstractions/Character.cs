using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Utils.ComponentInjection;
using Kugushev.Scripts.Game.AI;
using Kugushev.Scripts.Game.Features;
using Kugushev.Scripts.Game.Services;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.Characters.Abstractions
{
    public abstract class Character : Model, IMovable, IActive
    {
        [SerializeField] private bool isMoving = false;

        #region IMovable

        [ComponentInjection]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local", Justification = "Use Component Injection")]
        public INavigationService NavigationService { get; private set; }

        public bool IsMoving
        {
            get => isMoving;
            set => isMoving = value;
        }

        #endregion

        #region IActive
        public ICommander Commander { get; } = new Commander();

        #endregion

        protected override void Dispose(bool destroying) => Commander?.Dispose();
    }
}