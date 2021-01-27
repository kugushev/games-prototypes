using System;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Presentation.Events;
using Kugushev.Scripts.Presentation.PresentationModels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kugushev.Scripts.Presentation.Controllers
{
    [RequireComponent(typeof(XRController))]
    public class HandController : MonoBehaviour
    {
        [SerializeField] private PlanetEvent onHoverPlanet;
        [SerializeField] private PlanetEvent onHoverPlanetCancel;
        [SerializeField] private PlanetEvent onSelectPlanet;
        [SerializeField] private PlanetEvent onSelectPlanetCancel;
        [SerializeField] private MovingEvent onSelectedMove;

        private XRController _xrController;

        // todo: think about moving this logic to PlayerController
        private Planet _hoveredPlanet;
        private Planet _selectedPlanet;

        private void Awake()
        {
            _xrController = GetComponent<XRController>();
        }

        private void Update()
        {
            if (_xrController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out var isPressed) && isPressed)
            {
                if (!ReferenceEquals(_selectedPlanet, null))
                {
                    // todo: add temp object to identify finger
                    onSelectedMove.Invoke(this, _selectedPlanet, _xrController.modelTransform.position);
                }
                else if (!ReferenceEquals(_hoveredPlanet, null))
                {
                    onSelectPlanet.Invoke(this, _hoveredPlanet);
                    _selectedPlanet = _hoveredPlanet;
                }
            }
            else
            {
                if (!ReferenceEquals(_selectedPlanet, null))
                {
                    onSelectPlanetCancel.Invoke(this, _selectedPlanet);
                    _selectedPlanet = null;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Planet"))
            {
                var ppm = other.GetComponent<PlanetPresentationModel>();
                if (!ReferenceEquals(ppm, null))
                {
                    var planet = ppm.Planet;
                    if (!ReferenceEquals(_hoveredPlanet, null) && !ReferenceEquals(_hoveredPlanet, planet))
                    {
                        Debug.LogError($"Planet {_hoveredPlanet} is already selected. New {planet}");
                        return;
                    }

                    _hoveredPlanet = planet;
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
                    _hoveredPlanet = null;
                }
            }
        }
    }
}