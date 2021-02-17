using Kugushev.Scripts.Game.Common.Entities.Abstractions;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Managers;
using Kugushev.Scripts.Presentation.PresentationModels.Abstractions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.Presentation.PresentationModels
{
    public class FleetPresentationModel: BasePresentationModel
    {
        [FormerlySerializedAs("fleetManager")] [SerializeField] private Fleet fleet;
        protected override IModel Model => fleet;
    }
}