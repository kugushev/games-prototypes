using Kugushev.Scripts.Game.Models.Characters;
using Kugushev.Scripts.Game.Models.Characters.Abstractions;
using Kugushev.Scripts.Presentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PresentationModels
{
    public class CharacterPresentationModel : BasePresentationModel
    {
        [SerializeField] private Character model;
        protected override Model Model => model;
    }
}