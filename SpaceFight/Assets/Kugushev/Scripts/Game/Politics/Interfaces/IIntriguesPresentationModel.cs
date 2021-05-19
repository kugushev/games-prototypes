using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Politics.Interfaces
{
    public interface IIntriguesPresentationModel
    {
        ToggleGroup ToggleGroup { get; }
        void SelectCard(IntrigueCard? card);
    }
}