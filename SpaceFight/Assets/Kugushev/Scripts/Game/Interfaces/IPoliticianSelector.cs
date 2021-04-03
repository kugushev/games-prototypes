using Kugushev.Scripts.Game.Models;

namespace Kugushev.Scripts.Game.Interfaces
{
    public interface IPoliticianSelector
    {
        Politician SelectedPolitician { get; }
    }
}