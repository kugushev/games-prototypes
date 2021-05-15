using Kugushev.Scripts.Common.Models.Abstractions;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.MissionPresentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.MissionPresentation.PresentationModels
{
    public class PlanetPresentationModel : BasePresentationModel
    {
        private Planet? _planet;
        protected override IModel? Model => _planet;

        public override bool IsInPrefab => true;


        public Planet Planet
        {
            get
            {
                Asserting.NotNull(_planet);
                return _planet;
            }
            set
            {
                if (_planet is { })
                {
                    Debug.LogError($"Unable to set planet {value} because {_planet} is already set");
                    return;
                }

                _planet = value;
            }
        }
    }
}