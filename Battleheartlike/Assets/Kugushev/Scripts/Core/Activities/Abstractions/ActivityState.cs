namespace Kugushev.Scripts.Core.Activities.Abstractions
{
    public readonly struct ActivityState<TActive, TState>
        where TActive : IInteractable
        where TState : struct
    {
        public TActive Active { get; }
        public TState State { get; }

        public ActivityState(TActive active, TState state)
        {
            Active = active;
            State = state;
        }
    }
}