using System;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Presentation.Events;
using Kugushev.Scripts.Presentation.PresentationModels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kugushev.Scripts.Presentation.Controllers
{
    [RequireComponent(typeof(XRController))]
    public class HandController : MonoBehaviour
    {
        [SerializeField] private HandType handType;
        [SerializeField] private PlanetEvent onHoverPlanet;
        [SerializeField] private PlanetEvent onHoverPlanetCancel;
        [SerializeField] private HandEvent onSelect;
        [SerializeField] private HandEvent onSelectCancel;
        [SerializeField] private MovingEvent onMove;

        private XRController _xrController;
        private bool _triggerPressed;

        public HandType HandType => handType;

        private void Awake()
        {
            _xrController = GetComponent<XRController>();
        }

        private void Update()
        {
            if (_xrController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out var isPressed) && isPressed)
            {
                if (!_triggerPressed)
                {
                    onSelect.Invoke(this);
                    _triggerPressed = true;
                }
            }
            else if (_triggerPressed)
            {
                onSelectCancel.Invoke(this);
                _triggerPressed = false;
            }

            onMove.Invoke(this, _xrController.modelTransform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Planet"))
            {
                var ppm = other.GetComponent<PlanetPresentationModel>();
                if (!ReferenceEquals(ppm, null))
                {
                    var planet = ppm.Planet;
                    onHoverPlanet.Invoke(this, planet);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Planet"))
            {
                var ppm = other.GetComponent<PlanetPresentationModel>();
                if (!ReferenceEquals(ppm, null))
                {
                    onHoverPlanetCancel.Invoke(this, ppm.Planet);
                }
            }
        }
    }
}