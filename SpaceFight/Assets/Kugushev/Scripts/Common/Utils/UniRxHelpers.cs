using System;
using TMPro;
using UniRx;

namespace Kugushev.Scripts.Common.Utils
{
    public static class UniRxHelpers
    {
        public static IDisposable SubscribeToTextMeshPro(this IObservable<string> source, TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x);
        }
    }
}