using System;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Entities;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Presentation.Common.Utils;
using Kugushev.Scripts.Presentation.PresentationModels;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.Presentation.Components
{
    public class ArmyPresenter : MonoBehaviour
    {
        [SerializeField] private float minScale = 0.005f;
        [SerializeField] private float maxScale = 0.05f;
        [SerializeField] private Transform mesh;
        [SerializeField] private TextMeshProUGUI powerText;
        [SerializeField] private ParticleSystem projectilesParticleSystem;

        private FleetPresenter _owner;

        public void SetOwner(FleetPresenter owner)
        {
            if (!ReferenceEquals(_owner, null))
                Debug.LogError("Fleet is already specified");

            _owner = owner;
        }

        public Army Army { get; internal set; }

        public void SendFollowingOrder()
        {
            Army.Status = ArmyStatus.OnMatch;
        }

        private void Update()
        {
            Army.NextStep(Time.deltaTime);

            ApplyModelChanges();
        }

        private void ApplyModelChanges()
        {
            ApplyTransformChanges();

            powerText.text = StringBag.FromInt(Army.Power);

            ApplyFight();

            if (Army.Disbanded)
                _owner.ReturnArmyToPool(this);
        }

        private void ApplyTransformChanges()
        {
            var t = transform;

            t.position = Army.Position;
            t.rotation = Army.Rotation;
            mesh.localScale = GetAdjustedScale();
        }

        private void ApplyFight()
        {
            if (Army.Status == ArmyStatus.Fighting)
            {
                if (!projectilesParticleSystem.isPlaying)
                    projectilesParticleSystem.Play();
            }
            else if (!projectilesParticleSystem.isPlaying)
                projectilesParticleSystem.Stop();
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
                    Army.HandlePlanetVisiting(planet);
                }
            }

            if (other.CompareTag("Zone"))
            {
                Army.HandleCrash();
            }
        }
    }
}