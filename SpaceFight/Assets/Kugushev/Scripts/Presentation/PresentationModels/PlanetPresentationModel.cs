using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Models.Abstractions;
using Kugushev.Scripts.Presentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PresentationModels
{
    public class PlanetPresentationModel: BasePresentationModel
    {
        [SerializeField] private Planet model;
        protected override Model Model => model;
    }
}