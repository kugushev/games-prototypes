using Kugushev.Scripts.Components;
using Kugushev.Scripts.Models.Characters.Abstractions;
using Kugushev.Scripts.Models.Managers;
using Kugushev.Scripts.ValueObjects;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kugushev.Scripts.Controllers
{
    [RequireComponent(typeof(XRController))]
    public class HandController : MonoBehaviour
    {
        [SerializeField] private PlayableCharactersManager playableCharactersManager;
        [SerializeField] private PlayableCharacter character;
        private XRController _xrController;

        private void Awake()
        {
            _xrController = GetComponent<XRController>();
        }

        private void FixedUpdate()
        {
            //if (_xrController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool isPressed) && isPressed)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit,
                    Mathf.Infinity))
                {
                    var interactable = hit.collider.GetComponent<CharacterInteractable>();
                    Character passive = null;
                    if (!ReferenceEquals(null, interactable))
                        passive = interactable.Character;

                    var position = new Position(hit.point);
                    if (!playableCharactersManager.TryExecuteInteraction(character, passive, position))
                    {
                        // todo: show red line
                    }
                }
            }

            // if (_xrController.inputDevice.IsPressed(InputHelpers.Button.PrimaryButton, out bool button) && button)
            // {
            //     unit.Attack();
            // }
        }
    }
}