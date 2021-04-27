using System;
using Kugushev.Scripts.Common.Utils.Pooling;
using Zenject;

namespace Kugushev.Scripts.Common.Modes
{
    public static class ZenjectExtensions
    {
        public static SignalCopyBinder ToSignaledTransitionFromResolve<TParameters, TSignal>(
            this BindSignalToBinder<TSignal> binder, Func<TSignal, TParameters> map)
        {
            return binder.ToMethod((SignaledTransition<TParameters> obj, TSignal signal) =>
                {
                    var parameters = map(signal);
                    obj.Signalise(parameters);

                    if (signal is ISelfDespawning selfDespawning)
                        selfDespawning.DespawnSelf();
                })
                .FromResolve();
        }

        public static void SetupTransitiveSignal<TParameters>(this DiContainer container)
        {
            container.DeclareSignal<SignalToTransition<TParameters>>();
            container.BindMemoryPool<SignalToTransition<TParameters>, SignalToTransition<TParameters>.Pool>();
            container.BindSignal<SignalToTransition<TParameters>>()
                .ToSignaledTransitionFromResolve(s => s.Parameters);
        }
    }
}