using Kugushev.Scripts.Campaign.Core.ValueObjects;
using UnityEngine.UI;

namespace Kugushev.Scripts.Campaign.MissionSelection.Interfaces
{
    public interface IMissionSelectionPresentationModel
    {
        ToggleGroup ToggleGroup { get; }
        
        void SelectCard(MissionInfo? card);
    }
}