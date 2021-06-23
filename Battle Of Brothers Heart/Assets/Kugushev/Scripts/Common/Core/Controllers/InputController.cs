using System;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kugushev.Scripts.Common.Core.Controllers
{
    public class InputController
    {
        public event Action<Position>? GroundRightClick;
        public event Action<IInteractable>? InteractableRightClick;

        public void OnGroundClick(Vector2 position, PointerEventData.InputButton inputButton)
        {
            switch (inputButton)
            {
                case PointerEventData.InputButton.Right:
                    GroundRightClick?.Invoke(new Position(position));
                    break;

                default:
                    Debug.Log("Miss ground");
                    break;
            }
        }

        public void OnInteractableClick(IInteractable interactable, PointerEventData.InputButton inputButton)
        {
            switch (inputButton)
            {
                case PointerEventData.InputButton.Right:
                    InteractableRightClick?.Invoke(interactable);
                    break;

                default:
                    Debug.Log("Miss interactable");
                    break;
            }
        }
    }
}