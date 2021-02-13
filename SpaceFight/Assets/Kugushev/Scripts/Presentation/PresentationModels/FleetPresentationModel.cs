using Kugushev.Scripts.Game.Common.Entities.Abstractions;
using Kugushev.Scripts.Game.Missions.Managers;
using Kugushev.Scripts.Presentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PresentationModels
{
    public class FleetPresentationModel: BasePresentationModel
    {
        [SerializeField] private FleetManager fleetManager;
        protected override Model Model => fleetManager;
    }
}