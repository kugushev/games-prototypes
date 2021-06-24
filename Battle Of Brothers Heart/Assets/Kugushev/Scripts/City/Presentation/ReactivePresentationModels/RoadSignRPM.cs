using Kugushev.Scripts.City.Core.Models;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Presentation.Interfaces;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.City.Presentation.ReactivePresentationModels
{
    public class RoadSignRPM : MonoBehaviour, IInteractableOwner
    {
        [Inject] private RoadSign _model = default!;

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
    }
}