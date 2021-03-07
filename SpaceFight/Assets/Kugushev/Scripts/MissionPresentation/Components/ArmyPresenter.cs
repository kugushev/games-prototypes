using System.Linq;
using JetBrains.Annotations;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.MissionPresentation.PresentationModels;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.MissionPresentation.Components
{
    public class ArmyPresenter : MonoBehaviour
    {
        [SerializeField] private float minScale = 0.005f;
        [SerializeField] private float maxScale = 0.05f;
        [SerializeField] private Transform mesh;
        [SerializeField] private TextMeshProUGUI powerText;
        [SerializeField] private ParticleSystem siegeCannon;
        [SerializeField] private ParticleSystem[] fightCannons;

        [SerializeReference] private Army army;

        private FleetPresenter _fleet;

        public void SetOwner(FleetPresenter owner)
        {
            if (!ReferenceEquals(_fleet, null))
                Debug.LogError("Fleet is already specified");

            _fleet = owner;
        }

        [CanBeNull]
        public Army Army
        {
            get => army;
            internal set => army = value;
        }

        public void SendFollowingOrder()
        {
            if (Army != null)
                Army.Status = ArmyStatus.OnMatch;
        }

        private void OnDestroy()
        {
            army?.Dispose();
            army = null;
        }

        private void Update()
        {
            Army?.NextStep(Time.deltaTime);

            ApplyModelChanges();
        }

        private void ApplyModelChanges()
        {
            if (Army == null)
                return;

            ApplyTransformChanges();
            
            powerText.text = StringBag.FromInt(Army.Power);

            ApplyFight();
            ApplySiege();

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
            if (Army.Status == ArmyStatus.Fighting)
            {
                var targetsEnumerator = Army.TargetsUnderFire.GetEnumerator();
                foreach (var cannon in fightCannons)
                {
                    bool hasTarget = targetsEnumerator.MoveNext();
                    if (!hasTarget)
                    {
                        StopCannon(cannon);
                        continue;
                    }

                    var target = targetsEnumerator.Current;
                    if (target == null)
                    {
                        Debug.LogWarning("Target is null");
                        StopCannon(cannon);
                        continue;
                    }

                    if (!target.Active)
                    {
                        Debug.LogWarning("Target is not active");
                        StopCannon(cannon);
                        continue;
                    }

                    if (!cannon.isPlaying)
                        cannon.Play();
                    cannon.transform.LookAt(target.Position.Point);
                }
            }
            else
                foreach (var cannon in fightCannons)
                    StopCannon(cannon);

            void StopCannon(ParticleSystem cannon)
            {
                if (cannon.isPlaying)
                    cannon.Stop();
            }
        }

        private void ApplySiege()
        {
            if (Army.Status == ArmyStatus.OnSiege)
            {
                if (!siegeCannon.isPlaying)
                    siegeCannon.Play();

                var target = Army.TargetsUnderFire.FirstOrDefault();
                if (target != null)
                    siegeCannon.transform.LookAt(target.Position.Point);
            }
            else if (siegeCannon.isPlaying)
                siegeCannon.Stop();
        }

        private Vector3 GetAdjustedScale()
        {
            var relativePower = (float) Army.Power / GameplayConstants.SoftCapArmyPower;
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
                    Army?.HandlePlanetVisiting(planet);
                }
            }

            if (other.CompareTag("Zone"))
            {
                Army?.HandleCrash();
            }

            if (other.CompareTag("Army"))
            {
                var presenter = other.GetComponent<ArmyPresenter>();
                if (!ReferenceEquals(presenter, null))
                {
                    Army?.HandleArmyInteraction(presenter.Army);
                }
            }
        }
    }
}