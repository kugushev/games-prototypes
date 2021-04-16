using Kugushev.Scripts.Common.Models.Abstractions;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.MissionPresentation.PresentationModels.Abstractions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.MissionPresentation.PresentationModels
{
    public class FleetPresentationModel : BasePresentationModel
    {
        [FormerlySerializedAs("fleetManager")] [SerializeField]
        private Fleet? fleet;

        protected override IModel? Model => fleet;
    }
}