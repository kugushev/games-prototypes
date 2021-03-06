using System;
using Kugushev.Scripts.Common.Enums;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Presentation.Events;
using Kugushev.Scripts.Presentation.PresentationModels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.CommonUsages;

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
        private Slider _armySlider;
        private bool _triggerPressed;
        private Vector2 _joystickAxis;

        public Percentage ArmyPowerAllocated => new Percentage(_joystickAxis.x / 2f + 0.5f);
        public HandType HandType => handType;


        private void Awake()
        {
            _xrController = GetComponent<XRController>();
        }

        private void Update()
        {
            var inputDevice = _xrController.inputDevice;
            
            var newJoystickAxis = inputDevice.TryGetFeatureValue(primary2DAxis, out var joystickAxisOut)
                ? joystickAxisOut
                : default;
            if (_joystickAxis != newJoystickAxis && CanUseArmySlider())
            {
                _armySlider.value = ArmyPowerAllocated.Amount;
                _joystickAxis = newJoystickAxis;
            }

            if (inputDevice.IsPressed(InputHelpers.Button.Trigger, out var isPressed) && isPressed)
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

        private bool CanUseArmySlider()
        {
            if (!ReferenceEquals(_armySlider, null))
                return true;

            if (_xrController.modelTransform.childCount == 0)
                return false;

            _armySlider = _xrController.modelTransform.GetComponentInChildren<Slider>();
            return true;
        }
    }
}