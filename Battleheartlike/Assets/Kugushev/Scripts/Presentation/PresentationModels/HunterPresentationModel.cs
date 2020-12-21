using Kugushev.Scripts.Game.Models.Characters;
using Kugushev.Scripts.Game.Models.Characters.Abstractions;
using Kugushev.Scripts.Presentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PresentationModels
{
    public class HunterPresentationModel : BasePresentationModel
    {
        [SerializeField] private Hunter model;
        protected override Model Model => model;
    }
}