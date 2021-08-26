using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Fight
{
    [RequireComponent(typeof(VelocityMeasured))]
    public class HeroHead : MonoBehaviour
    {
        private const float ChargeVelocity = 0.05f;
        private const float ChargingSwing = 0.1f;
        private const float ChargeDistance = 8f;
        private const float ChargeCooldownSeconds = 1f;

        [SerializeField] private XRController rightController;
        [SerializeField] private TeleportationProvider teleportationProvider;

        [Inject] private Hero _hero;
        [Inject] private ChargeManager _chargeManager;

        private VelocityMeasured _velocityMeasured;
        private bool _charging;
        private Vector2 _swingStartPosition;
        private DateTime _chargeTime;
        private XRRayInteractor _rightRayInteractor;

        private void Awake()
        {
            _velocityMeasured = GetComponent<VelocityMeasured>();
            _rightRayInteractor = rightController.GetComponent<XRRayInteractor>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyProjectile"))
            {
                _hero.Suffer();
            }
        }

        private void Update()
        {
            if ((DateTime.Now - _chargeTime).TotalSeconds < ChargeCooldownSeconds)
            {
                return;
            }

            // if (Keyboard.current != null)
            // {
            //     if (Keyboard.current.spaceKey.isPressed)
            //     {
            //         _charging = true;
            //         _chargeStartPosition = GetSurfacePosition();
            //         print($"Charging at {_chargeStartPosition}");
            //     }
            // }

            var xrInput = rightController.inputDevice;
            if (xrInput.IsPressed(InputHelpers.Button.PrimaryButton, out var isPressed) && isPressed)
            {
                _rightRayInteractor.enabled = true;
                // rightController.SendHapticImpulse(1f, Time.deltaTime);
                if (!_charging)
                {
                    _swingStartPosition = GetSurfacePosition();
                    _charging = true;
                }
            }
            else
            {
                _rightRayInteractor.enabled = false;
                _charging = false;
                _swingStartPosition = default;
                _chargeManager.ActiveTarget = null;
            }
        }

        private void FixedUpdate()
        {
            if (_charging)
            {
                // print($"Velocity {_velocityMeasured.Velocity}");
                if (_velocityMeasured.Velocity >= ChargeVelocity)
                {
                    var swingStopPosition = GetSurfacePosition();
                    var distance = Vector3.Distance(_swingStartPosition, swingStopPosition);
                    // print($"Distance {distance}");
                    if (distance >= ChargingSwing)
                    {
                        // print("Charge");
                        Charge(swingStopPosition);
                    }
                }
            }
        }

        private void Charge(Vector2 swingStopPosition)
        {
            var startWorld = new Vector3(_swingStartPosition.x, 0, _swingStartPosition.y);
            var stopWorld = new Vector3(swingStopPosition.x, 0, swingStopPosition.y);
            
            Vector3 destination;
            if (_chargeManager.ActiveTarget is null)
                destination = GetSimpleChargeDestination(stopWorld, startWorld);
            else
            {
                var targetPosition = _chargeManager.ActiveTarget.transform.position;

                var startToTarget = targetPosition - startWorld;
                var startToStop = stopWorld - startWorld;
                var dot = Vector3.Dot(startToTarget, startToStop);
                const float cos45Deg = 0.7f;
                if (dot < cos45Deg)
                    destination = GetSimpleChargeDestination(startWorld, stopWorld);
                else
                {
                    var distance = startToTarget.magnitude - ZombieView.AttackDistance;
                    var delta = startToTarget.normalized * distance;
                    destination = startWorld + delta;
                }
            }

            teleportationProvider.QueueTeleportRequest(new TeleportRequest
            {
                destinationPosition = destination,
                destinationRotation = transform.rotation,
                requestTime = Time.time,
                matchOrientation = MatchOrientation.WorldSpaceUp
            });

            // Debug.DrawLine(
            //     transform.position,
            //     destination,
            //     Color.magenta, 30);

            _charging = false;
            _swingStartPosition = default;
            _chargeTime = DateTime.Now;
            _chargeManager.ActiveTarget = null;
        }

        private static Vector3 GetSimpleChargeDestination(Vector3 startWorld, Vector3 stopWorld)
        {
            var delta = (stopWorld - startWorld).normalized * ChargeDistance;
            return startWorld + delta;
        }

        private Vector2 GetSurfacePosition()
        {
            var position = transform.position;
            return new Vector2(position.x, position.z);
        }
    }
}