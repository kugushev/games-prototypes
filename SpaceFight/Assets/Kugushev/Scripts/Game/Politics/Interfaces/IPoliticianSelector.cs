using Kugushev.Scripts.Game.Core.Models;
using UniRx;

namespace Kugushev.Scripts.Game.Politics.Interfaces
{
    internal interface IPoliticianSelector
    {
        IReadOnlyReactiveProperty<IPolitician?> SelectedPolitician { get; }
    }
}