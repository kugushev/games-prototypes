using System;

namespace Kugushev.Scripts.Common.Utils.Pooling
{
    public interface IPoolable : IDisposable
    {
        void ClearState();
    }

    public interface IPoolable<in TState> : IPoolable
        where TState : struct
    {
        void SetState(TState state);
    }
}