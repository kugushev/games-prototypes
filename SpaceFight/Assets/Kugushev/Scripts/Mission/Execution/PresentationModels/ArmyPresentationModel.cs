using System;
using System.Linq;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Common.PresentationModels;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Execution.Interfaces;
using Kugushev.Scripts.Mission.Execution.PresentationModels;
using Kugushev.Scripts.Mission.Models;
using TMPro;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.MissionPresentation.Components
{
    public class ArmyPresentationModel : MonoBehaviour, IPoolable<Army, IFleetPresentationModel, IMemoryPool>,
        IDisposable
    {
        [SerializeField] private float minScale = 0.005f;
        [SerializeField] private float maxScale = 0.05f;
        [SerializeField] private Transform? mesh;
        [SerializeField] private TextMeshProUGUI? powerText;
        [SerializeField] private ParticleSystem? siegeCannon;
        [SerializeField] private ParticleSystem[]? fightCannons;

        private Army? _army;
        private IFleetPresentationModel? _fleet;
        private IMemoryPool? _pool;

        #region IPoolable, IDisposable, Factory

        void IPoolable<Army, IFleetPresentationModel, IMemoryPool>.OnSpawned(Army p1, IFleetPresentationModel p2,
            IMemoryPool p3)
        {
            _army = p1;
            _fleet = p2;
            _pool = p3;

            transform.position = _fleet.AssemblyPosition;
        }

        void IPoolable<Army, IFleetPresentationModel, IMemoryPool>.OnDespawned()
        {
            if (_fleet is { })
                transform.position = _fleet.AssemblyPosition;

            _army?.Dispose();

            _army = null;
            _fleet = null;
            _pool = null;
        }

        public void Dispose()
        {
            _pool?.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Army, IFleetPresentationModel, ArmyPresentationModel>
        {
        }

        #endregion

        public void SetOwner(FleetPresentationModel owner)
        {
            if (_fleet is { })
                Debug.LogError("Fleet is already specified");

            _fleet = owner;
        }

        public Army? Army
        {
            get => _army;
            internal set => _army = value;
        }

        public void SendFollowingOrder()
        {
            if (Army != null)
                Army.Status = ArmyStatus.OnMatch;
        }

        private void OnDestroy()
        {
            _army?.Dispose();
            _army = null;
        }

        private void Update()
        {
            Army?.NextStep(Time.deltaTime);

            ApplyModelChanges();
        }

        private void ApplyModelChanges()
        {
            Asserting.NotNull(powerText, _fleet, mesh);

            var model = Army;

            if (model == null)
                return;

            ApplyTransformChanges(model, mesh);

            powerText.text = StringBag.FromInt(Mathf.CeilToInt(model.Power));

            ApplyFight(model);
            ApplySiege(model);

            if (model.Disbanded)
                Dispose();
        }


        private void ApplyTransformChanges(Army model, Transform meshTransform)
        {
            var t = transform;

            t.position = model.Position.Point;
            t.rotation = model.Rotation;
            meshTransform.localScale = GetAdjustedScale(model);
        }

        private void ApplyFight(Army model)
        {
            Asserting.NotNull(fightCannons);

            if (model.Status == ArmyStatus.Fighting)
            {
                using var targetsEnumerator = model.GetTargetsUnderFire().GetEnumerator();

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

        private void ApplySiege(Army model)
        {
            Asserting.NotNull(siegeCannon);

            if (model.Status == ArmyStatus.OnSiege)
            {
                if (!siegeCannon.isPlaying)
                    siegeCannon.Play();

                var target = model.GetTargetsUnderFire().FirstOrDefault();
                if (target != null)
                    siegeCannon.transform.LookAt(target.Position.Point);
            }
            else if (siegeCannon.isPlaying)
                siegeCannon.Stop();
        }

        private Vector3 GetAdjustedScale(Army model)
        {
            var relativePower = model.Power / GameplayConstants.SoftCapArmyPower;
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
                var presenter = other.GetComponent<ArmyPresentationModel>();
                if (presenter is {Army: { }})
                {
                    Army?.HandleArmyInteraction(presenter.Army);
                }
            }
        }
    }
}