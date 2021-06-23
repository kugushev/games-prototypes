using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Presentation.Helpers;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Presentation.Interfaces;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels
{
    public class CityRPM : MonoBehaviour, IInteractableOwner
    {
        private City _model = default!;

        [Inject]
        private void Init(City city)
        {
            _model = city;
        }

        IInteractable IInteractableOwner.Interactable => _model;

        private void Awake()
        {
            var t = transform;
            t.position = new Vector3(
                _model.Position.Vector.x,
                _model.Position.Vector.y,
                t.parent.position.z
            );
        }


        public class Factory : PlaceholderFactory<City, CityRPM>
        {
        }
    }
}