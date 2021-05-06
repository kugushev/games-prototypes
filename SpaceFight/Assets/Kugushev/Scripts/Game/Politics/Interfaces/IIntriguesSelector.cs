using Kugushev.Scripts.Game.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Game.Interfaces
{
    public interface IIntriguesSelector
    {
        IReadOnlyReactiveProperty<IntrigueRecord?> SelectedIntrigue { get; }
    }
}