using System;
using Kugushev.Scripts.Core.Activities.Abstractions;
using Kugushev.Scripts.Core.Activities.Managers;
using Kugushev.Scripts.Core.ValueObjects;
using Kugushev.Scripts.Models.Characters.Abstractions;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kugushev.Scripts.Controllers
{
    [RequireComponent(typeof(XRController))]
    public class HandController : MonoBehaviour
    {
        [SerializeField] private InteractionsManager interactionsManager;
        [SerializeField] private PlayableCharacter character;
        private XRController _xrController;

        private void Awake()
        {
            _xrController = GetComponent<XRController>();
        }

        private void FixedUpdate()
        {
            if (_xrController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool isPressed) && isPressed)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit,
                    Mathf.Infinity))
                {
                    //var passives = hit.collider.GetComponents<IInteractable>(); how to take MODELS with high performance
                    var position = new Position(hit.point);
                    if (!interactionsManager.TryExecuteInteraction(character, Array.Empty<IInteractable>(), position))
                    {
                        // todo: show line
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