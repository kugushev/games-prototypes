using System;
using Kugushev.Scripts.Common.Utils.Pooling;
using Zenject;

namespace Kugushev.Scripts.Common.Utils
{
    public static class ZenjectExtensions
    {
        public static BindSignalFromBinder<TObject, TSignal> ToMethodAndDespawn<TSignal, TObject>(
            this BindSignalToBinder<TSignal> binder, Action<TObject, TSignal> handler)
            where TSignal : ISelfDespawning
        {
            return binder.ToMethod((TObject obj, TSignal signal) =>
            {
                handler(obj, signal);
                signal.DespawnSelf();
            });
        }
    }
}