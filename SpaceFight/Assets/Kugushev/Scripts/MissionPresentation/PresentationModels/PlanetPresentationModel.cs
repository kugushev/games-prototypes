using Kugushev.Scripts.Common.Models.Abstractions;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.MissionPresentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.MissionPresentation.PresentationModels
{
    public class PlanetPresentationModel : BasePresentationModel
    {
        protected override IModel Model => _planet;

        public override bool IsInPrefab => true;

        private Planet _planet;

        public Planet Planet
        {
            get => _planet;
            set
            {
                if (!ReferenceEquals(_planet, null))
                {
                    Debug.LogError($"Unable to set planet {value} because {_planet} is already set");
                    return;
                }

                _planet = value;
            }
        }
    }
}