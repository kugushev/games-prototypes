using System;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Presentation.PresentationModels;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Kugushev.Scripts.Presentation.Components
{
    public class ArmyPresenter : MonoBehaviour
    {
        [SerializeField] private float minScale = 0.005f;
        [SerializeField] private float maxScale = 0.05f;

        private FleetPresenter _owner;

        public void SetOwner(FleetPresenter owner)
        {
            if (!ReferenceEquals(_owner, null))
                Debug.LogError("Fleet is already specified");

            _owner = owner;
        }

        public Army Army { get; internal set; }

        public void Send()
        {
            StartCoroutine(Army.Send(() => Time.deltaTime));
        }

        private void Update()
        {
            var t = transform;

            t.position = Army.Position;
            t.rotation = Army.Rotation;
            t.localScale = GetAdjustedScale();

            if (Army.Disbanded)
                _owner.ReturnArmyToPool(this);
        }

        private Vector3 GetAdjustedScale()
        {
            var relativePower = (float) Army.Power / GameConstants.SoftCapArmyPower;
            var scale = Mathf.Lerp(minScale, maxScale, relativePower);
            return new Vector3(scale, scale, scale);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Planet"))
            {
                var ppm = other.GetComponent<PlanetPresentationModel>();
                if (!ReferenceEquals(ppm, null))
                {
                    var planet = ppm.Planet;
                    Army.HandlePlanetArriving(planet);
                }
            }
        }
    }
}