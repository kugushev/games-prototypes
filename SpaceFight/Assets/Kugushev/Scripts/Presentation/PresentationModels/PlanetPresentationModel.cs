using Kugushev.Scripts.Game.Common.Entities.Abstractions;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Presentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PresentationModels
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