using System;
using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Utils.ComponentInjection;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.AI.BehaviorTree;
using Kugushev.Scripts.Game.AI.BehaviorTree.Abstractions;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Features;
using Kugushev.Scripts.Game.Services;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.Characters.Abstractions
{
    [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer",
        Justification = "Defaults require for Serialized fields")]
    public abstract class Character : Model, ICharacter, IInteractionParty, IMovable
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private bool isMoving = false;

        #region ICharacter

        public IBehaviorTreeTask GetMovementBehavior(in Position target)
        {
            if (!NavigationService.TestDestination(in target))
                return null;

            return Pool.GetObject<MoveToTask, MoveToTask.State>(new MoveToTask.State(this, target));
        }

        public IBehaviorTreeTask GetAttackBehavior()
        {
            throw new NotImplementedException();
        }

        public IBehaviorTreeTask GetAssistBehavior()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IInteractionParty

        public abstract Faction Faction { get; }

        #endregion

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

        public IBehaviorTreeManager BehaviorTreeManager { get; } = new BehaviorTreeManager();

        #endregion

        protected ObjectsPool Pool => pool;
        protected override void Dispose(bool destroying) => BehaviorTreeManager?.Dispose();
    }
}