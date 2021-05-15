using System;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
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

        public static void InstallSignalAndBind<TParam, TSignal, TFactory, TReceiver>(this DiContainer container,
            Action<TReceiver, TSignal> bindHandler)
            where TFactory : PlaceholderFactory<TParam, TSignal>
            where TSignal : IPoolable<TParam, IMemoryPool>
        {
            container.DeclareSignal<TSignal>();
            container.InstallPoolable<TParam, TSignal, TFactory>();
            container.BindSignal<TSignal>()
                .ToMethod<TReceiver>((receiver, signal) =>
                {
                    bindHandler(receiver, signal);
                    if (signal is IDisposable disposable)
                        disposable.Dispose();
                })
                .FromResolve();
        }

        public static void InstallPoolable<TParam, TContract, TFactory>(this DiContainer container)
            where TFactory : PlaceholderFactory<TParam, TContract>
            where TContract : IPoolable<TParam, IMemoryPool>
        {
            container.BindFactory<TParam, TContract, TFactory>()
                .FromPoolableMemoryPool(x => x.WithInitialSize(2));
        }

        public static void InstallSignaledTransition<TParam>(this DiContainer container)
        {
            container.Bind<SignaledTransition<TParam>>().AsSingle();
            container.Bind<IParameterizedTransition<TParam>>().To<SignaledTransition<TParam>>().FromResolve();
        }

        public static void InstallExitState<TParam>(this DiContainer container)
        {
            container.Bind<ExitState<TParam>>().AsSingle();
            container.Bind<IParameterizedTransition<TParam>>().To<ExitState<TParam>>().FromResolve();
        }
    }
}