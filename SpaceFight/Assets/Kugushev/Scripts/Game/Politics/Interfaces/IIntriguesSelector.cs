using Kugushev.Scripts.Game.Core.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Game.Politics.Interfaces
{
    public interface IIntriguesSelector
    {
        IReadOnlyReactiveProperty<IntrigueCard?> SelectedIntrigue { get; }
    }
}