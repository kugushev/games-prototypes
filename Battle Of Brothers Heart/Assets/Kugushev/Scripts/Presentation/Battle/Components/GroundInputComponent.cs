using System;
using Kugushev.Scripts.Presentation.Battle.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class GroundInputComponent : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private InputController _inputController = default!;

        private Camera _camera = default!;

        private void Awake()
        {
            _camera = Camera.main!;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            var position = _camera.ScreenToWorldPoint(eventData.position);
            _inputController.OnGroundClick(position, eventData.button);
        }
    }
}