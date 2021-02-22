using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Presentation.Common.Utils;
using Kugushev.Scripts.Presentation.PresentationModels;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Components
{
    public class ArmyPresenter : MonoBehaviour
    {
        [SerializeField] private float minScale = 0.005f;
        [SerializeField] private float maxScale = 0.05f;
        [SerializeField] private Transform mesh;
        [SerializeField] private TextMeshProUGUI powerText;
        [SerializeField] private ParticleSystem projectilesParticleSystem;
        [SerializeReference] private Army army;

        private FleetPresenter _fleet;

        public void SetOwner(FleetPresenter owner)
        {
            if (!ReferenceEquals(_fleet, null))
                Debug.LogError("Fleet is already specified");

            _fleet = owner;
        }

        public Army Army
        {
            get => army;
            internal set => army = value;
        }

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
                _fleet.ReturnArmyToPool(this);
        }

        private void ApplyTransformChanges()
        {
            var t = transform;

            t.position = Army.Position.Point;
            t.rotation = Army.Rotation;
            mesh.localScale = GetAdjustedScale();
        }

        private void ApplyFight()
        {
            if (Army.Status == ArmyStatus.Fighting || Army.Status == ArmyStatus.OnSiege)
            {
                if (!projectilesParticleSystem.isPlaying)
                    projectilesParticleSystem.Play();

                if (Army.CurrentTarget != null)
                    projectilesParticleSystem.gameObject.transform.LookAt(Army.CurrentTarget.Position.Point);
            }
            else if (projectilesParticleSystem.isPlaying)
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

            if (other.CompareTag("Army"))
            {
                var presenter = other.GetComponent<ArmyPresenter>();
                if (!ReferenceEquals(presenter, null))
                {
                    Army.HandleArmyInteraction(presenter.Army);
                }
            }
        }
    }
}