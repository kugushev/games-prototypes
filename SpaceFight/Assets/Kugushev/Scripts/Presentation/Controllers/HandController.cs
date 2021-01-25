using System;
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
        [SerializeField] private PlanetEvent onTouchPlanet;
        [SerializeField] private PlanetEvent onReleasePlanet;
        
        private XRController _xrController;

        private void Awake()
        {
            _xrController = GetComponent<XRController>();
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Planet"))
            {
                var ppm = other.GetComponent<PlanetPresentationModel>();
                if (!ReferenceEquals(ppm, null))
                {
                    onTouchPlanet.Invoke(ppm.Planet);
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
                    onReleasePlanet.Invoke(ppm.Planet);
                }
            }            
        }
    }
}