using System;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Kugushev.Scripts.Common.Core.Controllers
{
    public class InputController : ITickable
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

        public void Tick()
        {
            // todo: move to TimeController
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.timeScale == 0)
                {
                    Debug.Log("Unpause");
                    Time.timeScale = 1;
                }
                else
                {
                    Debug.Log("Pause");
                    Time.timeScale = 0;
                }
            }
        }
    }
}