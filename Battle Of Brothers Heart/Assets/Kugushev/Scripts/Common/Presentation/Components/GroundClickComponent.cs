using Kugushev.Scripts.Common.Core.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Kugushev.Scripts.Common.Presentation.Components
{
    public class GroundClickComponent : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private InputController _inputController = default!;

        private Camera _camera = default!;

        private void Awake()
        {
            _camera = Camera.main!;
        }

        public void Lol()
        {
            print("Lol");
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            var position = _camera.ScreenToWorldPoint(eventData.position);
            _inputController.OnGroundClick(position, eventData.button);
        }
    }
}