using Kugushev.Scripts.Common.Core.Controllers;
using Kugushev.Scripts.Common.Presentation.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Kugushev.Scripts.Common.Presentation.Components
{
    [RequireComponent(typeof(IInteractableOwner))]
    public class InteractableClickComponent : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private InputController _inputController = default!;

        private IInteractableOwner _owner = default!;

        private void Awake()
        {
            _owner = GetComponent<IInteractableOwner>();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            print($"Click {_owner}");
            _inputController.OnInteractableClick(_owner.Interactable, eventData.button);
        }
    }
}