using Kugushev.Scripts.Game.Models;
using UniRx;

namespace Kugushev.Scripts.Game.Interfaces
{
    internal interface IPoliticianSelector
    {
        IReadOnlyReactiveProperty<IPolitician?> SelectedPolitician { get; }
    }
}