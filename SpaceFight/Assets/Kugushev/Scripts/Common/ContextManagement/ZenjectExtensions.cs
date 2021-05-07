using System;
using Zenject;

namespace Kugushev.Scripts.Common.ContextManagement
{
    public static class ZenjectExtensions
    {
        public static SignalCopyBinder ToSignaledTransitionFromResolve<TParameters, TSignal>(
            this BindSignalToBinder<TSignal> binder, Func<TSignal, TParameters> map) =>
            binder.ToMethod((SignaledTransition<TParameters> obj, TSignal signal) =>
                {
                    var parameters = map(signal);
                    obj.Signalise(parameters);

                    if (signal is IDisposable disposable)
                        disposable.Dispose();
                })
                .FromResolve();

        public static void InstallTransitiveSignal<TParameters>(this DiContainer container)
        {
            container.DeclareSignal<SignalToTransition<TParameters>>();
            container
                .BindFactory<TParameters, SignalToTransition<TParameters>, SignalToTransition<TParameters>.Factory>()
                .FromPoolableMemoryPool(x => x.WithInitialSize(2));
            container.BindSignal<SignalToTransition<TParameters>>()
                .ToSignaledTransitionFromResolve(s => s.Parameters);
        }
    }
}