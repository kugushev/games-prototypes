using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Utils.ComponentInjection;
using Kugushev.Scripts.Game.AI.DecisionMaking;
using Kugushev.Scripts.Game.Features;
using Kugushev.Scripts.Game.Services;

namespace Kugushev.Scripts.Game.Models.Characters.Abstractions
{
    public abstract class Character : Model, IMovable, IActive
    {
        #region IMovable

        [ComponentInjection]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local", Justification = "Use Component Injection")]
        public INavigationService NavigationService { get; private set; }

        public bool IsMoving { get; set; }

        #endregion

        #region IActive
        public IBehaviorTree BehaviorTree { get; } = new BehaviorTree();

        #endregion

        protected override void Dispose(bool destroying) => BehaviorTree?.Dispose();
    }
}