using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC
{
    [RequireComponent(typeof(VelocityMeasured))]
    public class HeroHead : MonoBehaviour
    {
        private const float ChargeVelocity = 0.05f;
        private const float ChargingSwing = 0.05f;
        private const float ChargeDistance = 8f;
        private const float ChargeCooldownSeconds = 2f;

        [SerializeField] private UnityEngine.XR.Interaction.Toolkit.XRController rightController;
        [SerializeField] private TeleportationProvider teleportationProvider;

        [Inject] private Hero _hero;

        private VelocityMeasured _velocityMeasured;
        private bool _charging;
        private Vector2 _chargeStartPosition;
        private DateTime _chargeTime;

        private void Awake()
        {
            _velocityMeasured = GetComponent<VelocityMeasured>();
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
                rightController.SendHapticImpulse(1f, Time.deltaTime);
                if (!_charging)
                {
                    _chargeStartPosition = GetSurfacePosition();
                    _charging = true;
                }
            }
            else
            {
                _charging = false;
                _chargeStartPosition = default;
            }
        }

        private void FixedUpdate()
        {
            if (_charging)
            {
                // print($"Velocity {_velocityMeasured.Velocity}");
                if (_velocityMeasured.Velocity >= ChargeVelocity)
                {
                    var chargeStopPosition = GetSurfacePosition();
                    var distance = Vector3.Distance(_chargeStartPosition, chargeStopPosition);
                    // print($"Distance {distance}");
                    if (distance >= ChargingSwing)
                    {
                        // print("Charge");
                        var startWorld = new Vector3(_chargeStartPosition.x, 0, _chargeStartPosition.y);
                        var stopWorld = new Vector3(chargeStopPosition.x, 0, chargeStopPosition.y);
                        var delta = (stopWorld - startWorld).normalized * ChargeDistance;
                        var destination = startWorld + delta;


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
                        _chargeStartPosition = default;
                        _chargeTime = DateTime.Now;
                    }
                }
            }
        }

        private Vector2 GetSurfacePosition()
        {
            var position = transform.position;
            return new Vector2(position.x, position.z);
        }
    }
}