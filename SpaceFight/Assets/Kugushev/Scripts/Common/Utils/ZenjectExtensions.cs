using System;
using Zenject;

namespace Kugushev.Scripts.Common.Utils
{
    public static class ZenjectExtensions
    {
        public static BindSignalFromBinder<TObject, TSignal> ToMethodAndDispose<TSignal, TObject>(
            this BindSignalToBinder<TSignal> binder, Action<TObject, TSignal> handler)
            where TSignal : IDisposable
        {
            return binder.ToMethod((TObject obj, TSignal signal) =>
            {
                handler(obj, signal);
                signal.Dispose();
            });
        }
    }
}