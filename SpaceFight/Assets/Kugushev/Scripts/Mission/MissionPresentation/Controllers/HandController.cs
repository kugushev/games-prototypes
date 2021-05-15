using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Enums;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.MissionPresentation.Events;
using Kugushev.Scripts.MissionPresentation.PresentationModels;
using Kugushev.Scripts.MissionPresentation.Widgets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.CommonUsages;

namespace Kugushev.Scripts.MissionPresentation.Controllers
{
    [RequireComponent(typeof(XRController))]
    public class HandController : MonoBehaviour
    {
        [SerializeField] private HandType handType;
        [SerializeField] private PlanetEvent? onHoverPlanet;
        [SerializeField] private PlanetEvent? onHoverPlanetCancel;
        [SerializeField] private HandEvent? onSelect;
        [SerializeField] private HandEvent? onSelectCancel;
        [SerializeField] private MovingEvent? onMove;
        [SerializeField] private HandEvent? onSurrenderClick;
        [SerializeField] private Camera? mainCamera;

        private XRController? _xrController;
        private HandWidget? _handWidget;
        private bool _triggerPressed;
        private Vector2 _joystickAxis;

        public Percentage ArmyPowerAllocated
        {
            get
            {
                //  _joystickAxis.x / 2f + 0.5f;
                float percentage;
                if (_joystickAxis.x < -0.75f) // full left
                    percentage = 0.1f;
                else if (_joystickAxis.x < -0.25f) // slight left
                    percentage = 0.25f;
                else if (_joystickAxis.x < 0.25f) // middle position
                    percentage = 0.5f;
                else if (_joystickAxis.x < 0.75f) // slight right
                    percentage = 0.75f;
                else // fill right
                    percentage = 1f;

                return new Percentage(percentage);
            }
        }

        public HandType HandType => handType;


        private void Awake()
        {
            _xrController = GetComponent<XRController>();
        }

        private void Update()
        {
            Asserting.NotNull(_xrController, mainCamera);

            if (!TryGetHand(_xrController, mainCamera, out var hand))
                return;

            var inputDevice = _xrController.inputDevice;

            var newJoystickAxis = inputDevice.TryGetFeatureValue(primary2DAxis, out var joystickAxisOut)
                ? joystickAxisOut
                : default;
            if (_joystickAxis != newJoystickAxis)
            {
                hand.UpdateSlider(ArmyPowerAllocated.Amount);
                _joystickAxis = newJoystickAxis;
            }

            if (inputDevice.IsPressed(InputHelpers.Button.Trigger, out var isPressed) && isPressed)
            {
                if (!_triggerPressed)
                {
                    onSelect?.Invoke(this);
                    _triggerPressed = true;
                }
            }
            else if (_triggerPressed)
            {
                onSelectCancel?.Invoke(this);
                _triggerPressed = false;
            }

            onMove?.Invoke(this, hand.IndexPointPosition);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Planet"))
            {
                var ppm = other.GetComponent<PlanetPresentationModel>();
                if (!ReferenceEquals(ppm, null))
                {
                    var planet = ppm.Planet;
                    onHoverPlanet?.Invoke(this, planet);
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
                    onHoverPlanetCancel?.Invoke(this, ppm.Planet);
                }
            }
        }

        private bool TryGetHand(XRController xrController, Camera cam,
            [NotNullWhen(true)] out HandWidget? handWidget)
        {
            if (!ReferenceEquals(_handWidget, null))
            {
                handWidget = _handWidget;
                return true;
            }

            if (xrController.modelTransform.childCount == 0)
            {
                handWidget = null;
                return false;
            }

            handWidget = xrController.modelTransform.GetComponentInChildren<HandWidget>();
            handWidget.Setup(cam);
            handWidget.SurrenderClick += OnSurrender;

            _handWidget = handWidget;
            return true;
        }

        private void OnSurrender() => onSurrenderClick?.Invoke(this);
    }
}