using Kugushev.Scripts.Game.Models;

namespace Kugushev.Scripts.Game.Interfaces
{
    internal interface IPoliticianSelector
    {
        Politician? SelectedPolitician { get; }
    }
}