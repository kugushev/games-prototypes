using JetBrains.Annotations;
using Kugushev.Scripts.Common.Pooling;
using Kugushev.Scripts.Core.AI.Objectives.Abstractions;

namespace Kugushev.Scripts.Core.Activities.Abstractions
{
    public abstract class Activity<TActive, TState> : Poolable<ActivityState<TActive, TState>>, IActivity
        where TActive : IInteractable
        where TState : struct
    {
        protected Activity(ObjectsPool objectsPool) : base(objectsPool) { }

        [CanBeNull]
        public abstract IObjective Act();
    }
}