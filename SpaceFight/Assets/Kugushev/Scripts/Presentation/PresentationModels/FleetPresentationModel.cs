using Kugushev.Scripts.Common.Entities.Abstractions;
using Kugushev.Scripts.Mission.Entities;
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